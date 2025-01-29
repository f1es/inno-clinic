using Appointment.Core.Models;

namespace Appointment.Core.Repositories;

public interface IResultRepository
{
	public Task<Result> GetByIdAsync(Guid id, bool trackChanges = false);
	public Task<IEnumerable<Result>> GetAllAsync();
}
