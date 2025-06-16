using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Offices.API.Extensions;
using Offices.Application.Extensions;
using Offices.Infrastructure.Extensions;
using Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

builder.Services.ConfigureApiLayer(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.ConfigureMediatr();
builder.Services.ConfigureAutomapper();
builder.Services.ConfigureRepositories();
//builder.Services.ConfigureOptions(builder.Configuration);


var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseCors("CorsPolicy");

app.MapHealthChecks("health");

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
