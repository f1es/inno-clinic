using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Services.Application.Mappers.Implementations;
using Services.Application.Mappers.Interfaces;
using Services.Application.MediatR.PipelineBehaviors;
using System.Reflection;

namespace Services.Application.Extensions;

public static class DependencyInjection
{
	private static void ConfigureMappers(this IServiceCollection services)
	{
		services.AddScoped<IServicesMapper, ServicesMapper>();
		services.AddScoped<IServiceCategoriesMapper, ServiceCategoriesMapper>();
	}

	private static void ConfigureMediatr(this IServiceCollection services)
	{
		services.AddMediatR(config =>
		{
			config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

			config.AddOpenBehavior(typeof(ValidationBehavior<,>));
		});
	}

	private static void ConfigureValidators(this IServiceCollection services)
	{
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
	}

	private static void ConfigureSerilog(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddSerilog();

		Log.Logger = new LoggerConfiguration()
		.ReadFrom.Configuration(configuration)
		.Enrich.FromLogContext()
		.CreateLogger();
	}

	public static void ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
	{
		services.ConfigureMappers();
		services.ConfigureMediatr();
		services.ConfigureValidators();
		services.ConfigureSerilog(configuration);
	}
}
