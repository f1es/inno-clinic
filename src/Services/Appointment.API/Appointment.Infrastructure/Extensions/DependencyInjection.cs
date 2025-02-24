using Appointment.Core.Repositories;
using Appointment.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Appointment.Infrastructure.Extensions;

public static class DependencyInjection
{
	private static void ConfigureRepositories(this IServiceCollection services)
	{
		services.AddScoped<IUnitOfWork, UnitOfWork>();
	}

	public static void ConfigureInfrastructureLayer(this IServiceCollection services)
	{
		services.AddScoped<IUnitOfWork, UnitOfWork>();
	}
}
