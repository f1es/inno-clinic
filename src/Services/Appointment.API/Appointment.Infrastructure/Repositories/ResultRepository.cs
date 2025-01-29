using Appointment.Core.Models;
using Appointment.Core.Repositories;
using Appointment.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Appointment.Infrastructure.Repositories;

public class ResultRepository : BaseRepository<Result>, IResultRepository
{
    public ResultRepository(AppointmentDbContext context) 
        : base(context)
    { }

	public async Task<IEnumerable<Result>> GetAllAsync() => await _context.Results.AsNoTracking().ToListAsync();

	public Task<Result> GetByIdAsync(Guid id, bool trackChanges = false)
	{
		var query = trackChanges ? _context.Results : _context.Results.AsNoTracking();
		return query.FirstOrDefaultAsync(x => x.Id == id);
	}
}
