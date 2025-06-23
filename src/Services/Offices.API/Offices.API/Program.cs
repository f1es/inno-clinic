using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Offices.API.Extensions;
using Offices.Application.Extensions;
using Offices.Infrastructure.Extensions;
using Shared.Middlewares;
using Serilog;
using Winton.Extensions.Configuration.Consul;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

var environement = builder.Environment.EnvironmentName;
builder.Configuration.AddConsul($"Offices/{environement}", options => options.AddConsulConfiguration(builder.Configuration));
builder.Configuration.AddConsul($"Shared/{environement}", options => options.AddConsulConfiguration(builder.Configuration));

builder.Services.ConfigureApiLayer(builder.Configuration);
builder.Services.ConfigureApplicationLayer(builder.Configuration);
builder.Services.ConfigureInfrastructureLayer(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseSerilogRequestLogging();

app.UseCors("CorsPolicy");

app.MapHealthChecks("health");

// Configure the HTTP request pipeline.
//if (!app.Environment.IsProduction())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
