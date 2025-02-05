using Appointment.Core.Repositories;
using Appointment.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Appointment.Infrastructure.Extensions;

public static class DependencyInjection
{
	public static void ConfigureRepositories(this IServiceCollection services)
	{
		services.AddScoped<IUnitOfWork, UnitOfWork>();
	}
}
