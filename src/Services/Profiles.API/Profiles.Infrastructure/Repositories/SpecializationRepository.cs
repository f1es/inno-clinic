using Microsoft.EntityFrameworkCore;
using Profiles.Core.Models;
using Profiles.Core.Parameters;
using Profiles.Core.Repositories;
using Profiles.Infrastructure.Context;
using Profiles.Infrastructure.Extensions;

namespace Profiles.Infrastructure.Repositories;

public class SpecializationRepository : BaseRepository<Specialization>, ISpecializationRepository
{
    public SpecializationRepository(ProfilesDbContext context)
        : base(context)
    {
        
    }

	public async Task<IEnumerable<Specialization>> GetAllAsync(RequestParameters requestParameters) => 
		await _context.Specializations
		.AsNoTracking()
		.Order(requestParameters.OrderQuery, x => x.SpecializationName)
		.Search(requestParameters.SearchTerm)
		.ToListAsync();

	public async Task<Specialization> GetByIdAsync(Guid id, bool trackChanges = false)
	{
		var query = trackChanges ? _context.Specializations : _context.Specializations.AsNoTracking();
		return await query.FirstOrDefaultAsync(x => x.Id == id);
	}
}
