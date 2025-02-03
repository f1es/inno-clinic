using Microsoft.EntityFrameworkCore;
using Profiles.Core.Models;
using Profiles.Core.Parameters;
using Profiles.Core.Repositories;
using Profiles.Infrastructure.Context;
using Profiles.Infrastructure.Extensions;

namespace Profiles.Infrastructure.Repositories;

public class DoctorRepository : BaseRepository<Doctor>, IDoctorRepository
{
    public DoctorRepository(ProfilesDbContext context)
		: base(context)
    {
        
    }

    public async Task<IEnumerable<Doctor>> GetAllAsync(RequestParameters requestParameters) => 
		await _context.Doctors
		.AsNoTracking()
		.Order(requestParameters.OrderQuery, x => x.FirstName)
		.Search(requestParameters.SearchTerm)
		.ToListAsync();

	public async Task<Doctor> GetByIdAsync(Guid id, bool trackChanges = false)
	{
		var query = trackChanges ? _context.Doctors : _context.Doctors.AsNoTracking();
		return await query.FirstOrDefaultAsync(x => x.Id == id);
	}
}
