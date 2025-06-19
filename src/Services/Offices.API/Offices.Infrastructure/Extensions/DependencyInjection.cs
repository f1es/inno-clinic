using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Offices.Core.Cache;
using Offices.Core.Repositories;
using Offices.Infrastructure.Cache;
using Offices.Infrastructure.Context;
using Offices.Infrastructure.HealthChecks;
using Offices.Infrastructure.Options;
using Offices.Infrastructure.Repositories;

namespace Offices.Infrastructure.Extensions;

public static class DependencyInjection
{
	public static void ConfigureInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
	{
		services.ConfigureRepositories();
		services.ConfigureHealthChecks(configuration);
	}

	private static void ConfigureRepositories(this IServiceCollection services)
	{
		services.AddSingleton<IMongoClient>(sp =>
		{
			var options = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
			return new MongoClient(options.ConnectionString);
		});

		services.AddSingleton<OfficesContext>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<ICacheService, CacheService>();
	}

	private static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
	{
		var redisSettings = configuration.GetSection("RedisSettings").Get<RedisSettings>();

		services.AddHealthChecks()
			.AddRedis(redisSettings.Server, name: "redis", failureStatus: HealthStatus.Unhealthy)
			.AddCheck<MongoDbHealthCheck>("mongodb")
			.AddCheck("self", () => HealthCheckResult.Healthy());
	}
}
