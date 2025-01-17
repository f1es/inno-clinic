using Microsoft.Extensions.DependencyInjection;
using Offices.Core.Repositories;
using Offices.Infrastructure.Repositories;

namespace Offices.Infrastructure.Extensions;

public static class DependencyInjection
{
	public static void ConfigureRepositories(this IServiceCollection services)
	{
		services.AddScoped<IUnitOfWork, UnitOfWork>();
	}
}
