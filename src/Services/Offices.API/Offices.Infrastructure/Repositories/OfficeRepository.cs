using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Offices.Core.Models;
using Offices.Core.Repositories;
using Offices.Infrastructure.Context;

namespace Offices.Infrastructure.Repositories;

public class OfficeRepository : IOfficeRepository
{
	private readonly OfficesContext _context;
    public OfficeRepository(OfficesContext context)
    {
        _context = context;
    }

	public async Task CreateAsync(Office entity)
	{
		await _context.Offices.InsertOneAsync(entity);
	}

	public async Task DeleteAsync(Office entity)
	{
		var filter = Builders<Office>.Filter.Eq(x => x.Id, entity.Id);
		await _context.Offices.DeleteOneAsync(filter);
	}

	public async Task UpdateAsync(Office entity)
	{
		var filter = Builders<Office>.Filter.Eq(x => x.Id, entity.Id);
		await _context.Offices.ReplaceOneAsync(filter, entity);
	}

	public async Task<IEnumerable<Office>> GetAllAsync()
	{
		var filter = Builders<Office>.Filter.Empty;
		return await _context.Offices.Find(filter).ToListAsync();
	}

	public async Task<Office> GetByIdAsync(Guid id)
	{
		var filter = Builders<Office>.Filter.Eq(x => x.Id, id);
		return await _context.Offices.Find(filter).FirstOrDefaultAsync();
	}
}
