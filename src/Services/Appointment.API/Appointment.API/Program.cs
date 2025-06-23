using Appointment.API.Extensions;
using Appointment.Application.Extensions;
using Appointment.Infrastructure.Extensions;
using Appointment.Infrastructure.Hubs;
using Shared.Middlewares;
using Winton.Extensions.Configuration.Consul;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment.EnvironmentName;
builder.Configuration.AddConsul($"Appointments/{environment}", options => options.AddConsulConfiguration(builder.Configuration));
builder.Configuration.AddConsul($"Shared/{environment}", options => options.AddConsulConfiguration(builder.Configuration));

// Add services to the container.
builder.Services.ConfigureApiLayer(builder.Configuration);
builder.Services.ConfigureInfrastructureLayer(builder.Configuration);
builder.Services.ConfigureApplicationLayer(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseCors("CorsPolicy");

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHub<NotificationHub>("/hub/notifications");

app.MapControllers();

app.Run();

public partial class Program;