using FluentValidation;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
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

	private static void ConfigureMassTransit(this IServiceCollection services)
	{
		services.AddMassTransit(config =>
		{
			config.UsingRabbitMq((context, config) =>
			{
				config.Host("localhost", "/", host =>
				{
					host.Username("guest");
					host.Password("guest");
				});
			});
		});
	}

	public static void ConfigureApplication(this IServiceCollection services)
	{
		services.ConfigureMappers();
		services.ConfigureMediatr();
		services.ConfigureValidators();
		services.ConfigureMassTransit();
	}
}
