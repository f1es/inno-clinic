using Microsoft.EntityFrameworkCore;
using Profiles.Core.Models;
using Profiles.Core.Parameters;
using Profiles.Core.Repositories;
using Profiles.Infrastructure.Context;
using Profiles.Infrastructure.Extensions;

namespace Profiles.Infrastructure.Repositories;

public class ReceptionistRepository : BaseRepository<Receptionist>, IReceptionistRepository
{
    public ReceptionistRepository(ProfilesDbContext context) 
        : base(context)
    {
        
    }

	public async Task<IEnumerable<Receptionist>> GetAllAsync(RequestParameters requestParameters) => 
		await _context.Receptionists
		.AsNoTracking()
		.Order(requestParameters.OrderQuery, x => x.FirstName)
		.Search(requestParameters.SearchTerm)
		.ToListAsync();
	
	public async Task<Receptionist> GetByIdAsync(Guid id, bool trackChanges = false)
	{
		var query = trackChanges ? _context.Receptionists : _context.Receptionists.AsNoTracking();
		return await query.FirstOrDefaultAsync(x => x.Id == id);
	}
}
