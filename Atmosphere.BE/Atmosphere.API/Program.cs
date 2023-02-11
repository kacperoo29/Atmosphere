using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Atmosphere.Application.Config;
using Atmosphere.Application.Configuration;
using Atmosphere.Application.Notfications;
using Atmosphere.Application.Readings.Commands;
using Atmosphere.Application.Services;
using Atmosphere.Core.Enums;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using Atmosphere.Core.Validation;
using Atmosphere.Services;
using Atmosphere.Services.Auth;
using Atmosphere.Services.Consts;
using Atmosphere.Services.Notifications;
using Atmosphere.Services.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        );
    });

builder.Services.AddCors();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Atmosphere API", Version = "v1" });
    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        }
    );
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                new List<string>()
            }
        }
    );
});

builder.Services.AddScoped<IMongoClient>(
    opt => new MongoClient(builder.Configuration.GetConnectionString("mongodb"))
);

builder.Services.AddScoped<IMongoCollection<ConfigurationEntry>>(
    opt =>
        opt.GetRequiredService<IMongoClient>()
            .GetDatabase(MongoDbConsts.DbName)
            .GetCollection<ConfigurationEntry>(MongoDbConsts.ConfigurationCollectionName)
);

builder.Services.AddScoped<IMongoCollection<Reading>>(
    opt =>
        opt.GetRequiredService<IMongoClient>()
            .GetDatabase(MongoDbConsts.DbName)
            .GetCollection<Reading>(MongoDbConsts.ReadingsCollectionName)
);

builder.Services.AddScoped<IMongoCollection<BaseUser>>(
    opt =>
        opt.GetRequiredService<IMongoClient>()
            .GetDatabase(MongoDbConsts.DbName)
            .GetCollection<BaseUser>(MongoDbConsts.UsersCollectionName)
);

builder.Services.AddScoped<IMongoCollection<Device>>(
    opt =>
        opt.GetRequiredService<IMongoClient>()
            .GetDatabase(MongoDbConsts.DbName)
            .GetCollection<Device>(MongoDbConsts.UsersCollectionName)
);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
        JwtBearerDefaults.AuthenticationScheme,
        opt =>
        {
            var config = builder.Configuration.GetSection("JWT");
            opt.RequireHttpsMetadata = false;

            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = config.GetValue<string>("Issuer"),
                ValidateAudience = true,
                ValidAudience = config.GetValue<string>("Audience"),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(0),
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(config.GetValue<string>("SecretKey"))
                )
            };

            opt.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;
                    if (
                        !string.IsNullOrEmpty(accessToken)
                        && (path.StartsWithSegments("/api/websocket"))
                    )
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        }
    );

builder.Services.AddAuthorization(opt =>
{
    opt.DefaultPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();

    opt.AddPolicy(
        nameof(UserRole.Admin),
        policy => policy.RequireClaim(ClaimTypes.Role, nameof(UserRole.Admin))
    );

    opt.AddPolicy(
        nameof(UserRole.Device),
        policy => policy.RequireClaim(ClaimTypes.Role, nameof(UserRole.Device))
    );
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IReadingRepository, ReadingRepository>();
builder.Services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IConfigService, ConfigService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReadingValidator, ReadingValidator>();

builder.Services.AddScoped(opt =>
{
    using var scope = opt.CreateScope();
    var config = scope.ServiceProvider.GetRequiredService<IConfigService>();
    var types = config.GetNotificationTypesAsync().GetAwaiter().GetResult();
    var notificationHub = scope.ServiceProvider.GetRequiredService<IWebSocketHub<Notification>>();

    INotificationService notificationService = new NotificationService();
    foreach (var type in types)
        switch (type)
        {
            case NotificationType.Email:
                notificationService = new EmailNotificationServiceDecorator(
                    notificationService,
                    config
                );
                break;
            case NotificationType.WebSocket:
                notificationService = new WebSocketNotificationServiceDecorator(
                    notificationService,
                    notificationHub
                );
                break;
            default:
                throw new Exception($"Invalid notification type {type}");
        }

    return notificationService;
});

builder.Services.AddScoped<ITokenService, JwtTokenProvider>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddMediatR(typeof(CreateReadingHandler).Assembly);
builder.Services.AddAutoMapper(typeof(CreateReadingHandler).Assembly);

builder.Services.AddSingleton<IWebSocketHub<Notification>, NotificationsHub>();
builder.Services.AddSingleton<IWebSocketHub<Device>, DeviceHub>();

builder.Services.AddScoped<IDeviceService, DeviceService>();

BsonClassMap.RegisterClassMap<ConfigurationEntry>(cm =>
{
    cm.AutoMap();
    cm.SetIgnoreExtraElements(true);
});
BsonClassMap.RegisterClassMap<EmailConfiguration>();
BsonClassMap.RegisterClassMap<BaseUser>(cm =>
{
    cm.AutoMap();
    cm.AddKnownType(typeof(Device));
    cm.AddKnownType(typeof(User));
});

BsonClassMap.RegisterClassMap<BaseModel>(cm =>
{
    cm.AutoMap();
    cm.MapIdMember(c => c.Id).SetIdGenerator(GuidGenerator.Instance);
});

BsonSerializer.RegisterSerializer(new EnumSerializer<ReadingType>(BsonType.String));

BsonSerializer.RegisterSerializer(new LinqSerializer<Func<Reading, bool>>());

ConventionRegistry.Register(
    "Ignore extra elements",
    new ConventionPack { new IgnoreExtraElementsConvention(true) },
    t => true
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Add admin account if not exists
    using var scope = app.Services.CreateScope();
    var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
    var user = userRepo.FindAsync(u => u.Username == "admin").GetAwaiter().GetResult();
    if (user?.Count() == 0)
    {
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        var admin = User.Create("admin", "secretPassword");
        admin.Activate();
        admin.MakeAdmin();
        userService.CreateUserAsync(admin).GetAwaiter().GetResult();
    }
}

app.UseCors(opt => opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseWebSockets();

app.Run();
