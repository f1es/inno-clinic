namespace Offices.Core.Cache;

public interface ICacheService
{
	public Task<T> ExecuteOrGetFromCacheAsync<T>(string cacheKey, Func<Task<T>> getDataFunc, CancellationToken cancellationToken = default) where T : class;
	public Task RemoveFromCacheAsync(string cacheKey, CancellationToken cancellationToken = default);
}
