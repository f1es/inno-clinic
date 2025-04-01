using Microsoft.Extensions.DependencyInjection;
using Offices.Core.Cache;
using Offices.Core.Repositories;
using Offices.Infrastructure.Cache;
using Offices.Infrastructure.Context;
using Offices.Infrastructure.Repositories;

namespace Offices.Infrastructure.Extensions;

public static class DependencyInjection
{
	public static void ConfigureRepositories(this IServiceCollection services)
	{
		services.AddSingleton<OfficesContext>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<ICacheService, CacheService>();
	}
}
