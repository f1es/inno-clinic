using Authorization.Infrastructure.Extensions;
using Authorization.Application.Extensions;
using Authorization.API.Extensions;
using Authorization.API.Builders;
using Shared.Middlewares;
using Winton.Extensions.Configuration.Consul;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.ConfigureApiLayer(builder.Configuration);
builder.Services.ConfigureInfrastructureLayer(builder.Configuration);
builder.Services.ConfigureApplicationLayer();	

var environment = builder.Environment.EnvironmentName;
builder.Configuration.AddConsul($"Auth/{environment}", options => options.AddConsulConfiguration(builder.Configuration));

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseCors("CorsPolicy");

app.MapHealthChecks("health", HealthCheckOptionsBuilder.Build());

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
