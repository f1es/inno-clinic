using Appointment.Core.Models;

namespace Appointment.Core.Repositories;

public interface IResultRepository : IBaseRepository<Result>
{
	public Task<Result> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool trackChanges = false);
	public Task<IEnumerable<Result>> GetAllAsync(CancellationToken cancellationToken);
}
