using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Atmoshpere.Application.Services;
using Atmoshpere.Services.Auth;
using Atmosphere.Application;
using Atmosphere.Application.Config;
using Atmosphere.Application.Configuration;
using Atmosphere.Application.Readings.Commands;
using Atmosphere.Application.Services;
using Atmosphere.Core.Enums;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using Atmosphere.Services.Auth;
using Atmosphere.Services.Consts;
using Atmosphere.Services.Notifications;
using Atmosphere.Services.Repositories;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Atmosphere API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
});
});

builder.Services.AddScoped<IMongoClient>((opt) =>
    new MongoClient(builder.Configuration.GetConnectionString("mongodb"))
);

builder.Services.AddScoped<IMongoCollection<ConfigurationEntry>>((opt) =>
    opt.GetRequiredService<IMongoClient>()
        .GetDatabase(MongoDbConsts.DbName)
        .GetCollection<ConfigurationEntry>(MongoDbConsts.ConfigurationCollectionName)
);

builder.Services.AddScoped<IMongoCollection<Reading>>((opt) =>
    opt.GetRequiredService<IMongoClient>()
        .GetDatabase(MongoDbConsts.DbName)
        .GetCollection<Reading>(MongoDbConsts.ReadingsCollectionName)
);

builder.Services.AddScoped<IMongoCollection<BaseUser>>((opt) =>
    opt.GetRequiredService<IMongoClient>()
        .GetDatabase(MongoDbConsts.DbName)
        .GetCollection<BaseUser>(MongoDbConsts.UsersCollectionName)
);

builder.Services.AddScoped<IMongoCollection<Device>>((opt) =>
    opt.GetRequiredService<IMongoClient>()
        .GetDatabase(MongoDbConsts.DbName)
        .GetCollection<Device>(MongoDbConsts.UsersCollectionName)
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, (opt) =>
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("SecretKey"))),
        };
    });

builder.Services.AddAuthorization(opt =>
{
    opt.DefaultPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IReadingRepository, ReadingRepository>();
builder.Services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IConfigService, ConfigService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<INotificationService>((opt) =>
{
    using var scope = opt.CreateScope();
    var config = scope.ServiceProvider.GetRequiredService<IConfigService>();
    var types = config.GetNotificationTypes().GetAwaiter().GetResult();

    INotificationService notificationService = new NotificationService();
    foreach (var type in types) {
        switch (type) {
            case NotificationType.Email:
                notificationService = new EmailNotificationServiceDecorator(notificationService, config);
                break;
            default:
                throw new Exception($"Invalid notification type {type}");
        }
    }

    return notificationService;
});

builder.Services.AddScoped<ITokenService, JwtTokenProvider>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddMediatR(typeof(CreateReadingHandler).Assembly);

BsonClassMap.RegisterClassMap<EmailConfiguration>();
BsonClassMap.RegisterClassMap<BaseUser>(cm => 
{
    cm.AutoMap();
    cm.SetIsRootClass(true);
    cm.AddKnownType(typeof(Device));
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
