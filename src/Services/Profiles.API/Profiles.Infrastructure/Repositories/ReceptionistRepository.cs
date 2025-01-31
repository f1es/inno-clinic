using Microsoft.EntityFrameworkCore;
using Profiles.Core.Models;
using Profiles.Core.Repositories;
using Profiles.Infrastructure.Context;

namespace Profiles.Infrastructure.Repositories;

public class ReceptionistRepository : BaseRepository<Receptionist>, IReceptionistRepository
{
    public ReceptionistRepository(ProfilesDbContext context) 
        : base(context)
    {
        
    }

	public async Task<IEnumerable<Receptionist>> GetAllAsync() => await _context.Receptionists.AsNoTracking().ToListAsync();
	
	public async Task<Receptionist> GetByIdAsync(Guid id, bool trackChanges = false)
	{
		var query = trackChanges ? _context.Receptionists : _context.Receptionists.AsNoTracking();
		return await query.FirstOrDefaultAsync(x => x.Id == id);
	}
}
