using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Offices.Core.Cache;
using Offices.Infrastructure.Options;
using System.Text.Json;

namespace Offices.Infrastructure.Cache;

public class RedisCacheService : ICacheService
{
	private readonly IDistributedCache _distributedCache;
	private readonly RedisSettings _redisSettings;

	public RedisCacheService(IDistributedCache distributedCache, IOptions<RedisSettings> redisSettings)
	{
		_distributedCache = distributedCache;
		_redisSettings = redisSettings.Value;
	}

	public async Task<T> ExecuteOrGetFromCacheAsync<T>(
		string cacheKey, 
		Func<Task<T>> getDataFunc, 
		CancellationToken cancellationToken = default) where T : class
	{
		var obj = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);

		if (obj is not null)
		{
			return JsonSerializer.Deserialize<T>(obj);
		}

		var data = await getDataFunc();

		if (data is not null)
		{
			var cacheTime = TimeSpan.FromMinutes(_redisSettings.CacheTimeMinutes);
			var cacheOptions = new DistributedCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = cacheTime
			};

			var json = JsonSerializer.Serialize(data);
			await _distributedCache.SetStringAsync(cacheKey, json, cacheOptions, cancellationToken);
		}

		return data;
	}

	public async Task RemoveFromCacheAsync(string cacheKey, CancellationToken cancellationToken = default)
	{
		var cachedObject = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);
		if (cachedObject is not null)
		{
			await _distributedCache.RemoveAsync(cacheKey, cancellationToken);
		}
	}
}
