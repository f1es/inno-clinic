using Offices.Core.Models;

namespace Offices.Core.Repositories;

public interface IRecepcionistRepository : IBaseRepository<Receptionist>
{
	public Task<Receptionist> GetByIdAsync(Guid id, bool trackChanges = false);
	public Task<Receptionist> GetByOfficeIdAsync(Guid officeId, bool trackChanges = false);
	public Task<IEnumerable<Receptionist>> GetAllAsync();
}
