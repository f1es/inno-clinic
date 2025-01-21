using MongoDB.Driver;
using Offices.Core.Models;
using Offices.Core.Repositories;
using Offices.Infrastructure.Context;

namespace Offices.Infrastructure.Repositories;

public class ReceptionistRepository : IRecepcionistRepository
{
	private readonly OfficesContext _context;
    public ReceptionistRepository(OfficesContext context)
    {
        _context = context;
    }

	public async Task CreateAsync(Receptionist entity)
	{
		await _context.Receptionists.InsertOneAsync(entity);
	}

	public async Task DeleteAsync(Receptionist entity)
	{
		var filter = Builders<Receptionist>.Filter.Eq(x => x.Id, entity.Id);
		await _context.Receptionists.DeleteOneAsync(filter);
	}

	public async Task UpdateAsync(Receptionist entity)
	{
		var filter = Builders<Receptionist>.Filter.Eq(x => x.Id, entity.Id);
		await _context.Receptionists.ReplaceOneAsync(filter, entity);
	}

	public async Task<IEnumerable<Receptionist>> GetAllAsync()
	{
		var filter = Builders<Receptionist>.Filter.Empty;
		return await _context.Receptionists.Find(filter).ToListAsync();
	}

	public async Task<Receptionist> GetByIdAsync(Guid id)
	{
		var filter = Builders<Receptionist>.Filter.Eq(x => x.Id, id);
		return await _context.Receptionists.Find(filter).FirstOrDefaultAsync();
	}

	public async Task<Receptionist> GetByOfficeIdAsync(Guid officeId)
	{
		var filter = Builders<Receptionist>.Filter.Eq(x => x.OfficeId, officeId);
		return await _context.Receptionists.Find(filter).FirstOrDefaultAsync();
	}
}
