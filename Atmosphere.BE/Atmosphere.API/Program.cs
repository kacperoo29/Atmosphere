using System.Text.Json;
using System.Text.Json.Serialization;

using Atmosphere.Application.Readings.Commands;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using Atmosphere.Services.Consts;
using Atmosphere.Services.Repositories;

using MediatR;

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

builder.Services.AddScoped<IMongoCollection<Reading>>((opt) =>
    builder.Services.BuildServiceProvider()
        .GetRequiredService<IMongoClient>()
        .GetDatabase(MongoDbConsts.DbName)
        .GetCollection<Reading>(MongoDbConsts.ReadingsCollectionName)
);

builder.Services.AddScoped<IReadingRepository, ReadingRepository>();

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

app.Run();
