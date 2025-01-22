using Services.Core.Models;

namespace Services.Core.Repositories;

public interface IServiceCategoryRepository
{
	public Task<ServiceCategory> GetByIdAsync(Guid id);
	public Task<IEnumerable<ServiceCategory>> GetAllAsync();
}
