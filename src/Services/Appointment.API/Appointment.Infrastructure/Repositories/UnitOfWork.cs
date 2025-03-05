using Appointment.Core.Repositories;
using Appointment.Infrastructure.Context;

namespace Appointment.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
	private readonly Lazy<IAppointmentRepository> _appointmentRepository;
	private readonly Lazy<IResultRepository> _resultRepository;
	private readonly AppointmentDbContext _context;

    public IAppointmentRepository AppointmentRepository => _appointmentRepository.Value;
	public IResultRepository ResultRepository => _resultRepository.Value;

	public UnitOfWork(AppointmentDbContext context)
	{
		_appointmentRepository = new Lazy<IAppointmentRepository>(() => new AppointmentRepository(context));

		_resultRepository = new Lazy<IResultRepository>(() => new ResultRepository(context));

		_context = context;
	}

	public async Task SaveAsync(CancellationToken cancellationToken) => await _context.SaveChangesAsync(cancellationToken);
}
