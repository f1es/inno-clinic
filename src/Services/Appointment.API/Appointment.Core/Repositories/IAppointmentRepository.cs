namespace Appointment.Core.Repositories;

public interface IAppointmentRepository
{
	public Task<Models.Appointment> GetByIdAsync(Guid id, bool trackChanges = false);
	public Task<IEnumerable<Models.Appointment>> GetAllAsync();
}
