using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Application.Mappers.Implementations;
using Services.Application.Mappers.Interfaces;
using Services.Application.MediatR.PipelineBehaviors;
using System.Reflection;

namespace Services.Application.Extensions;

public static class DependencyInjection
{
	public static void ConfigureMappers(this IServiceCollection services)
	{
		services.AddScoped<IServicesMapper, ServicesMapper>();
		services.AddScoped<IServiceCategoriesMapper, ServiceCategoriesMapper>();
	}

	public static void ConfigureMediatr(this IServiceCollection services)
	{
		services.AddMediatR(config =>
		{
			config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

			config.AddOpenBehavior(typeof(ValidationBehavior<,>));
		});
	}

	public static void ConfigureValidators(this IServiceCollection services)
	{
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
	}
}
