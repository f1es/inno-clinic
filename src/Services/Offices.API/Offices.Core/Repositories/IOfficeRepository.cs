using Offices.Core.Models;

namespace Offices.Core.Repositories;

public interface IOfficeRepository : IBaseRepository<Office>
{
	public Task<Office> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
	public Task<IEnumerable<Office>> GetAllAsync(CancellationToken cancellationToken = default);
}
