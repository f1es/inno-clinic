using MongoDB.Driver;
using Offices.Core.Cache;
using Offices.Core.Models;
using Offices.Core.Repositories;
using Offices.Infrastructure.Context;

namespace Offices.Infrastructure.Repositories;

public class OfficeRepository : IOfficeRepository
{
	private readonly OfficesContext _context;
	private readonly ICacheService _cacheService;

	private const string OfficesCacheKeyPrefix = "offices.api-";
	private const string OfficesCollectionCacheKey = $"{OfficesCacheKeyPrefix}offices";

	public OfficeRepository(
		OfficesContext context,
		ICacheService cacheService)
	{
		_context = context;
		_cacheService = cacheService;
	}

	public async Task CreateAsync(Office entity, CancellationToken cancellationToken = default)  
	{
		await _context.Offices.InsertOneAsync(entity, options: null, cancellationToken);
		await _cacheService.RemoveFromCacheAsync(OfficesCollectionCacheKey, cancellationToken);
	}
	public async Task DeleteAsync(Office entity, CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{OfficesCacheKeyPrefix}{entity.Id}";
		await _cacheService.RemoveFromCacheAsync(cacheKey, cancellationToken);
		await _cacheService.RemoveFromCacheAsync(OfficesCollectionCacheKey, cancellationToken);

		await _context.Offices.DeleteOneAsync(Builders<Office>.Filter.Eq(x => x.Id, entity.Id));
	}

	public async Task UpdateAsync(Office entity, CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{OfficesCacheKeyPrefix}{entity.Id}";
		await _cacheService.CacheAsync(cacheKey, entity, cancellationToken);
		await _cacheService.RemoveFromCacheAsync(OfficesCollectionCacheKey, cancellationToken);

		await _context.Offices.ReplaceOneAsync(Builders<Office>.Filter.Eq(x => x.Id, entity.Id), entity);
	}

	public async Task<IEnumerable<Office>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		var cachedOffices = await _cacheService.GetFromCacheAsync<IEnumerable<Office>>(OfficesCollectionCacheKey, cancellationToken);
		if (cachedOffices is not null)
		{
			return cachedOffices;
		}

		var offices = (await _context.Offices.FindAsync(Builders<Office>.Filter.Empty)).ToList();
		await _cacheService.CacheAsync(OfficesCollectionCacheKey, offices, cancellationToken);

		return offices;
	}
		
	public async Task<Office> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var cacheKey = $"{OfficesCacheKeyPrefix}{id}";
		var cachedOffice = await _cacheService.GetFromCacheAsync<Office>(cacheKey, cancellationToken);
		if (cachedOffice is not null)
		{
			return cachedOffice;
		}

		var office = (await _context.Offices.FindAsync(Builders<Office>.Filter.Eq(x => x.Id, id))).FirstOrDefault();
		await _cacheService.CacheAsync(cacheKey, office, cancellationToken);

		return office;
	}
}
