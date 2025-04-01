using Appointment.Core.Repositories;
using Appointment.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Appointment.Infrastructure.Repositories;

public class AppointmentRepository : BaseRepository<Core.Models.Appointment>, IAppointmentRepository
{
    public AppointmentRepository(AppointmentDbContext context) 
		: base(context)
    { }

    public async Task<IEnumerable<Core.Models.Appointment>> GetAllAsync(CancellationToken cancellationToken) => await _context.Appointments.AsNoTracking().ToListAsync(cancellationToken);

	public async Task<IEnumerable<Core.Models.Appointment>> GetAllByDayAsync(DateOnly day, CancellationToken cancellationToken) =>
		await _context.Appointments.AsNoTracking().Where(x => x.Date == day).ToListAsync(cancellationToken);

	public async Task<Core.Models.Appointment> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool trackChanges = false)
	{
		var query = trackChanges ? _context.Appointments : _context.Appointments.AsNoTracking();
		return await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
	}

	public async Task RemoveServiceId(Guid serviceId) => 
		await _context.Appointments.Where(x => x.ServiceId == serviceId).ExecuteUpdateAsync(x => x.SetProperty(p => p.ServiceId, (Guid?)null));
}
