using Serilog;
using Services.API.Extensions;
using Services.Application.Extensions;
using Services.Infrastructure.Extensions;
using Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureApi(builder.Configuration);
builder.Services.ConfigureInfrastructure();
builder.Services.ConfigureApplication(builder.Configuration);

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

Log.Information("Application started");

app.Run();
