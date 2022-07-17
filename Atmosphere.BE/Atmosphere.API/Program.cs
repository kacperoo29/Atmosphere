using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Atmosphere.Application.Readings.Commands;
using Atmosphere.Application.Services;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using Atmosphere.Services.Consts;
using Atmosphere.Services.Notifications;
using Atmosphere.Services.Repositories;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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

builder.Services.AddScoped<IReadingRepository, ReadingRepository>();
builder.Services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
builder.Services.AddScoped<INotificationService>((opt) =>
{
    using var scope = opt.CreateScope();
    var config = scope.ServiceProvider.GetRequiredService<IConfigurationRepository>();
    var type = config.Get(NotificationTypes.NOTIFICATION_TYPE_KEY).Result ?? NotificationTypes.Email;
    switch (type)
    {
        case NotificationTypes.Email:
            return scope.ServiceProvider.GetRequiredService<EmailNotificationService>();
        default:
            throw new Exception($"Unknown notification type: {type}");
    }
});

builder.Services.AddMediatR(typeof(CreateReading).Assembly);

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

// Set default values for configuration entries
app.Lifetime.ApplicationStarted.Register(async () =>
{
    using var scope = app.Services.CreateScope();
    var config = scope.ServiceProvider.GetRequiredService<IConfigurationRepository>();
    var type = config.Get(NotificationTypes.NOTIFICATION_TYPE_KEY).Result ?? NotificationTypes.Email;
    await config.Set(NotificationTypes.NOTIFICATION_TYPE_KEY, type);

    var emailConfig = config.Get(EmailNotificationService.EMAIL_CONFIG_KEY).Result;
    if (emailConfig == null)
    {
        var emailSection = builder.Configuration.GetSection("EmailSettings");
        await config.Set(EmailNotificationService.EMAIL_CONFIG_KEY, new EmailConfiguration
        {
            SmtpServer = emailSection.GetValue<string>("SmtpServer"),
            SmtpPort = emailSection.GetValue<int>("SmtpPort"),
            SmtpUsername = emailSection.GetValue<string>("SmtpUsername"),
            SmtpPassword = emailSection.GetValue<string>("SmtpPassword"),
            EmailAddress = emailSection.GetValue<string>("EmailAddress"),
            ServerEmailAddress = emailSection.GetValue<string>("ServerEmailAddress")
        });
    }
});

app.Run();
