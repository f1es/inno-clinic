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

	public async Task CreateAsync(Receptionist entity) => await _context.Receptionists.InsertOneAsync(entity);
	public async Task DeleteAsync(Receptionist entity) => await _context.Receptionists.DeleteOneAsync(Builders<Receptionist>.Filter.Eq(x => x.Id, entity.Id));
	public async Task UpdateAsync(Receptionist entity) => await _context.Receptionists.ReplaceOneAsync(Builders<Receptionist>.Filter.Eq(x => x.Id, entity.Id), entity);
	public async Task<IEnumerable<Receptionist>> GetAllAsync() => await (await _context.Receptionists.FindAsync(Builders<Receptionist>.Filter.Empty)).ToListAsync();
	public async Task<Receptionist> GetByIdAsync(Guid id) => 
		(await _context.Receptionists.FindAsync(Builders<Receptionist>.Filter.Eq(x => x.Id, id))).FirstOrDefault();
	public async Task<Receptionist> GetByOfficeIdAsync(Guid officeId) => 
		(await _context.Receptionists.FindAsync(Builders<Receptionist>.Filter.Eq(x => x.OfficeId, officeId))).FirstOrDefault();
}
