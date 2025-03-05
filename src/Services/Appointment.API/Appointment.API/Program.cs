using Appointment.API.Extensions;
using Appointment.Application.Extensions;
using Appointment.Infrastructure.Extensions;
using Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwaggerGen();
builder.Services.ConfigureDbContext(builder);
builder.Services.ConfigureMappers();
builder.Services.ConfigureServices();
builder.Services.ConfigureRepositories();

var app = builder.Build();

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

app.Run();

public partial class Program;