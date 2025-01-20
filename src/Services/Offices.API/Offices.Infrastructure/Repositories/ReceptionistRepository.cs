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
		return await _context.Receptionists.ToListAsync();
	}

	public async Task<Receptionist> GetByIdAsync(Guid id, bool trackChanges = false)
	{
		if (trackChanges)
		{
			return await _context
				.Receptionists
				.FirstOrDefaultAsync(x => x.Id.Equals(id));
		}
		else
		{
			return await _context
				.Receptionists
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id.Equals(id));
		}
	}

	public async Task<Receptionist> GetByOfficeIdAsync(Guid officeId, bool trackChanges = false)
	{
		if (trackChanges)
		{
			return await _context
				.Receptionists
				.FirstOrDefaultAsync(x => x.OfficeId.Equals(officeId));
		}
		else
		{
			return await _context
				.Receptionists
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.OfficeId.Equals(officeId));
		}
	}
}
