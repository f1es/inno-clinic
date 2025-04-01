using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Offices.Core.Cache;
using Offices.Infrastructure.Options;
using System.Text.Json;

namespace Offices.Infrastructure.Cache;

public class CacheService : ICacheService
{
	private readonly IDistributedCache _distributedCache;
	private readonly RedisSettings _redisSettings;

	public CacheService(IDistributedCache distributedCache, IOptions<RedisSettings> redisSettings)
	{
		_distributedCache = distributedCache;
		_redisSettings = redisSettings.Value;
	}

	public async Task CacheAsync(string cacheKey, object obj, CancellationToken cancellationToken = default)
	{
		if (obj is null)
		{
			return;
		}

		var cacheTime = TimeSpan.FromMinutes(_redisSettings.CacheTimeMinutes);
		var cacheOptions = new DistributedCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = cacheTime
		};

		await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(obj), cacheOptions, cancellationToken);
	}

	public async Task<T> GetFromCacheAsync<T>(string cacheKey, CancellationToken cancellationToken = default) where T : class 
	{
		var cachedOffices = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);
		return cachedOffices is null ? null : JsonSerializer.Deserialize<T>(cachedOffices);
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
