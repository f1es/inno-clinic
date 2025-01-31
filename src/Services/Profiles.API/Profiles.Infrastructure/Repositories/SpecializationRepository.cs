using Microsoft.EntityFrameworkCore;
using Profiles.Core.Models;
using Profiles.Core.Repositories;
using Profiles.Infrastructure.Context;

namespace Profiles.Infrastructure.Repositories;

public class SpecializationRepository : BaseRepository<Specialization>, ISpecializationRepository
{
    public SpecializationRepository(ProfilesDbContext context)
        : base(context)
    {
        
    }

	public async Task<IEnumerable<Specialization>> GetAllAsync() => await _context.Specializations.AsNoTracking().ToListAsync();

	public async Task<Specialization> GetByIdAsync(Guid id, bool trackChanges = false)
	{
		var query = trackChanges ? _context.Specializations : _context.Specializations.AsNoTracking();
		return await query.FirstOrDefaultAsync(x => x.Id == id);
	}
}
