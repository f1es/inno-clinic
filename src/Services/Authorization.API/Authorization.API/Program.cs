using Authorization.Infrastructure.Extensions;
using Authorization.Application.Extensions;
using Authorization.API.Extensions;
using Authorization.API.Builders;
using Shared.Middlewares;
using Winton.Extensions.Configuration.Consul;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var environment = builder.Environment.EnvironmentName;
builder.Configuration.AddConsul($"Auth/{environment}", options => options.AddConsulConfiguration(builder.Configuration));
builder.Configuration.AddConsul($"Shared/{environment}", options => options.AddConsulConfiguration(builder.Configuration));

builder.Services.ConfigureApiLayer(builder.Configuration);
builder.Services.ConfigureInfrastructureLayer(builder.Configuration);
builder.Services.ConfigureApplicationLayer();	

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseCors("CorsPolicy");

app.MapHealthChecks("health", HealthCheckOptionsBuilder.Build());

if (!app.Environment.IsProduction())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
