using Offices.Core.Cache;
using Offices.Core.Models;

namespace Offices.Infrastructure.Cache;

public class OfficesCacheService : IOfficesCacheService
{
	private const string OfficesCacheKeyPrefix = "offices.api-";
	private const string OfficesCollectionCacheKey = $"{OfficesCacheKeyPrefix}offices";

	private readonly ICacheService _cacheService;

	public OfficesCacheService(ICacheService cacheService)
	{
		_cacheService = cacheService;
	}

	public async Task<ICollection<Office>> GetAllOfficesAsync(Func<Task<ICollection<Office>>> getDataFunc, CancellationToken cancellationToken = default) =>
		await _cacheService.ExecuteOrGetFromCacheAsync(OfficesCollectionCacheKey, getDataFunc, cancellationToken);

	public async Task<Office> GetOfficeAsync(Guid id, Func<Task<Office>> getDataFunc, CancellationToken cancellationToken = default) =>
		await _cacheService.ExecuteOrGetFromCacheAsync($"{OfficesCacheKeyPrefix}{id}", getDataFunc, cancellationToken);

	public Task RemoveAllOfficesFromCacheAsync(CancellationToken cancellationToken = default) => 
		_cacheService.RemoveFromCacheAsync(OfficesCollectionCacheKey, cancellationToken);

	public Task RemoveFromCacheAsync(Guid id, CancellationToken cancellationToken = default) =>
		_cacheService.RemoveFromCacheAsync($"{OfficesCacheKeyPrefix}{id}", cancellationToken);
}
