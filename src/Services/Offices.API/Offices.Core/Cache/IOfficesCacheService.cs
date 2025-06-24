using Offices.Core.Models;

namespace Offices.Core.Cache;

public interface IOfficesCacheService
{
	public Task<Office> GetOfficeAsync(Guid id, Func<Task<Office>> getDataFunc, CancellationToken cancellationToken = default);
	public Task<ICollection<Office>> GetAllOfficesAsync(Func<Task<ICollection<Office>>> getDataFunc, CancellationToken cancellationToken = default);

	public Task RemoveFromCacheAsync(Guid id, CancellationToken cancellationToken = default);
	public Task RemoveAllOfficesFromCacheAsync(CancellationToken cancellationToken = default);
}
