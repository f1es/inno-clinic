using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Offices.Infrastructure.Options;

namespace Offices.Infrastructure.HealthChecks;

public class MongoDbHealthCheck : IHealthCheck
{
	private readonly IMongoClient _mongoClient;
	private readonly MongoDbSettings _options;

	public MongoDbHealthCheck(IMongoClient mongoClient, IOptions<MongoDbSettings> options)
	{
		_mongoClient = mongoClient;
		_options = options.Value;
	}

	public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
	{
		try
		{
			var database = _mongoClient.GetDatabase(_options.DatabaseName);
			var command = new BsonDocument("ping", 1);
			await database.RunCommandAsync<BsonDocument>(command);
			return HealthCheckResult.Healthy("MongoDB is running");
		}
		catch (Exception ex)
		{
			return HealthCheckResult.Unhealthy("MongoDB is unavailable", ex);
		}
	}
}
