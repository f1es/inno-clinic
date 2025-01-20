using Microsoft.Extensions.DependencyInjection;
using Offices.Application.MapperProfiles;
using System.Reflection;

namespace Offices.Application.Extensions;

public static class DependencyInjection
{
	public static void ConfigureAutomapper(this IServiceCollection services)
	{
		services.AddAutoMapper(config =>
		{
			config.AddProfile<OfficeMapperProfile>();
			config.AddProfile<ReceptionistMapperProfile>();
		});
	}

	public static void ConfigureMediatr(this IServiceCollection services)
	{
		services.AddMediatR(config =>
		{
			config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
		});
	}
}
