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

	public async Task CreateAsync(Office entity) => await _context.Offices.InsertOneAsync(entity);

	public async Task DeleteAsync(Office entity) => await _context.Offices.DeleteOneAsync(Builders<Office>.Filter.Eq(x => x.Id, entity.Id));

	public async Task UpdateAsync(Office entity) => await _context.Offices.ReplaceOneAsync(Builders<Office>.Filter.Eq(x => x.Id, entity.Id), entity);

	public async Task<IEnumerable<Office>> GetAllAsync() => (await _context.Offices.FindAsync(Builders<Office>.Filter.Empty)).ToList();

	public async Task<Office> GetByIdAsync(Guid id) => (await _context.Offices.FindAsync(Builders<Office>.Filter.Eq(x => x.Id, id))).FirstOrDefault();
}
