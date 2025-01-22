using Services.Core.Models;

namespace Services.Core.Repositories;

public interface IServiceRepository
{
	public Task<Service> GetByIdAsync(Guid id);
	public Task<IEnumerable<Service>> GetAllAsync();
}
