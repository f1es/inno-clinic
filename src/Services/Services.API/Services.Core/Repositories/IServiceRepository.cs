using Services.Core.Models;

namespace Services.Core.Repositories;

public interface IServiceRepository : IBaseRepository<Service>
{
	public Task<Service> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool trackChanges = false);
	public Task<IEnumerable<Service>> GetAllAsync(CancellationToken cancellationToken);
}
