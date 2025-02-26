using Appointment.Infrastructure.Context;
using Appointment.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace Appointment.API.Extensions;

public static class DependencyInjection
{
	private static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<AppointmentDbContext>(options =>
		{
			options.UseNpgsql(configuration.GetConnectionString("NpsSql"));
		});
	}

	private static void ConfigureControllers(this IServiceCollection services)
	{
		services.AddControllers()
			.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
			});
	}

	private static void ConfigureSwaggerGen(this IServiceCollection services)
	{
		services.AddSwaggerGen(options =>
		{
			options.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" });
			options.MapType<TimeOnly>(() => new OpenApiSchema { Type = "string", Format = "time", Pattern = "00:00:00" });
		});
	}

	private static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<RabbitmqOptions>(configuration.GetSection("RabbitmqOptions"));
	}

	public static void ConfigureApiLayer(this IServiceCollection services, IConfiguration configuration)
	{
		services.ConfigureDbContext(configuration);
		services.ConfigureControllers();
		services.ConfigureSwaggerGen();
		services.AddEndpointsApiExplorer();
		services.ConfigureOptions(configuration);
	}
}
