namespace Offices.Core.Cache;

public interface ICacheService
{
	public Task CacheAsync(string cacheKey, object obj, CancellationToken cancellationToken = default);
	public Task RemoveFromCacheAsync(string cacheKey, CancellationToken cancellationToken = default);
	public Task<T> GetFromCacheAsync<T>(string cacheKey, CancellationToken cancellationToken = default) where T : class;
}
