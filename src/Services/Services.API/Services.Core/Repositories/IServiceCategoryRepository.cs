using Services.Core.Models;

namespace Services.Core.Repositories;

public interface IServiceCategoryRepository : IBaseRepository<ServiceCategory>
{
	public Task<ServiceCategory> GetByIdAsync(Guid id, bool trackChanges = false);
	public Task<IEnumerable<ServiceCategory>> GetAllAsync();
}
