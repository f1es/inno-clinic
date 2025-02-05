using Appointment.Core.Repositories;
using Appointment.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Appointment.Infrastructure.Repositories;

public class AppointmentRepository : BaseRepository<Core.Models.Appointment>, IAppointmentRepository
{
    public AppointmentRepository(AppointmentDbContext context) 
		: base(context)
    { }

    public async Task<IEnumerable<Core.Models.Appointment>> GetAllAsync() => await _context.Appointments.AsNoTracking().ToListAsync();

	public async Task<Core.Models.Appointment> GetByIdAsync(Guid id, bool trackChanges = false)
	{
		var query = trackChanges ? _context.Appointments : _context.Appointments.AsNoTracking();
		return await query.FirstOrDefaultAsync(x => x.Id == id);
	}
}
