namespace Appointment.Core.Repositories;

public interface IUnitOfWork
{
	public IAppointmentRepository AppointmentRepository { get; }
	public IResultRepository ResultRepository { get; }
	public Task SaveAsync();
}
