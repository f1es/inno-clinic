using Microsoft.EntityFrameworkCore;
using Profiles.Core.Models;
using Profiles.Core.Parameters;
using Profiles.Core.Repositories;
using Profiles.Infrastructure.Context;
using Profiles.Infrastructure.Extensions;

namespace Profiles.Infrastructure.Repositories;

public class PatientRepository : BaseRepository<Patient>, IPatientRepository
{
    public PatientRepository(ProfilesDbContext context) 
        : base(context) 
    {
        
    }

	public async Task<IEnumerable<Patient>> GetAllAsync(RequestParameters requestParameters) => 
		await _context.Patients
		.AsNoTracking()
		.Order(requestParameters.OrderQuery, x => x.FirstName)
		.Search(requestParameters.SearchTerm)
		.ToListAsync();

	public async Task<Patient> GetByIdAsync(Guid id, bool trackChanges = false)
	{
		var query = trackChanges ? _context.Patients : _context.Patients.AsNoTracking();
		return await query.FirstOrDefaultAsync(x => x.Id == id);
	}
}
