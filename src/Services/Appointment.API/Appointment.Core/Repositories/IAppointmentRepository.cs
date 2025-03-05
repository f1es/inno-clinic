namespace Appointment.Core.Repositories;

public interface IAppointmentRepository : IBaseRepository<Models.Appointment>
{
	public Task<Models.Appointment> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool trackChanges = false);
	public Task<IEnumerable<Models.Appointment>> GetAllAsync(CancellationToken cancellationToken);
	public Task RemoveServiceId(Guid serviceId);
}
