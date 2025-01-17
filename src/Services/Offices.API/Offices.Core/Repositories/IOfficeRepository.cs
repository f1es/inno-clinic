using Offices.Core.Models;

namespace Offices.Core.Repositories;

public interface IOfficeRepository : IBaseRepository<Office>
{
	public Task<Office> GetByIdAsync(Guid id, bool trackChanges = false);
	public Task<IEnumerable<Office>> GetAllAsync();
}
