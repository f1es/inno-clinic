using Microsoft.EntityFrameworkCore;
using Offices.Core.Models;
using Offices.Core.Repositories;
using Offices.Infrastructure.Context;

namespace Offices.Infrastructure.Repositories;

public class ReceptionistRepository : BaseRepository<Receptionist>, IRecepcionistRepository
{
    public ReceptionistRepository(OfficesDbContext context)
        : base (context)
    {
        
    }

	public async Task<IEnumerable<Receptionist>> GetAllAsync()
	{
		return await _context.Receptionists.AsNoTracking().ToListAsync();
	}

	public async Task<Receptionist> GetByIdAsync(Guid id, bool trackChanges = false)
	{
		var table = _context.Receptionists;
		var receptionist = trackChanges ?  table.FirstOrDefaultAsync(x => x.Id == id) : table.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
		return await receptionist;
	}

	public async Task<Receptionist> GetByOfficeIdAsync(Guid officeId, bool trackChanges = false)
	{
		var table = _context.Receptionists;
		var receptionist = trackChanges ? table.FirstOrDefaultAsync(x => x.OfficeId == officeId) : table.AsNoTracking().FirstOrDefaultAsync(x => x.OfficeId == officeId);
		return await receptionist;
	}
}
