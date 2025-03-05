using Microsoft.EntityFrameworkCore;
using Profiles.Core.Models;
using Profiles.Core.Parameters;
using Profiles.Core.Repositories;
using Profiles.Core.Utility;
using Profiles.Infrastructure.Context;
using Profiles.Infrastructure.Extensions;

namespace Profiles.Infrastructure.Repositories;

public class SpecializationRepository : BaseRepository<Specialization>, ISpecializationRepository
{
    public SpecializationRepository(ProfilesDbContext context)
        : base(context)
    {
        
    }

	public async Task<PagedList<Specialization>> GetAllAsync(RequestParameters requestParameters) => 
		await _context.Specializations
		.AsNoTracking()
		.Order(requestParameters.OrderQuery, x => x.SpecializationName)
		.Search(requestParameters.SearchTerm)
		.PaginateAsync(requestParameters.Page, requestParameters.PageSize);

	public async Task<Specialization> GetByIdAsync(Guid id, bool trackChanges = false)
	{
		var query = trackChanges ? _context.Specializations : _context.Specializations.AsNoTracking();
		return await query.FirstOrDefaultAsync(x => x.Id == id);
	}
}
