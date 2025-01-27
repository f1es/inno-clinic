using Offices.Core.Models;

namespace Offices.Core.Repositories;

public interface IOfficeRepository : IBaseRepository<Office>
{
	public Task<Office> GetByIdAsync(Guid id);
	public Task<IEnumerable<Office>> GetAllAsync();
}
