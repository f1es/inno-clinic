using Microsoft.Extensions.DependencyInjection;
using Services.Application.Mappers.Implementations;
using Services.Application.Mappers.Interfaces;

namespace Services.Application.Extensions;

public static class DependencyInjection
{
	public static void ConfigureMappers(this IServiceCollection services)
	{
		services.AddScoped<IServicesMapper, ServicesMapper>();
		services.AddScoped<IServiceCategoriesMapper, ServiceCategoriesMapper>();
	}
}
