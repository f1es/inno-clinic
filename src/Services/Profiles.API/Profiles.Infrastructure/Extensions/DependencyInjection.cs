using Microsoft.Extensions.DependencyInjection;
using Profiles.Core.Repositories;
using Profiles.Infrastructure.Repositories;

namespace Profiles.Infrastructure.Extensions;

public static class DependencyInjection
{
	public static void ConfigureRepositories(this IServiceCollection services)
	{
		services.AddScoped<IUnitOfWork, UnitOfWork>();
	}
}
