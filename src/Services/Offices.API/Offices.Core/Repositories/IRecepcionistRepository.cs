using Offices.Core.Models;

namespace Offices.Core.Repositories;

public interface IRecepcionistRepository : IBaseRepository<Receptionist>
{
	public Task<Receptionist> GetByIdAsync(Guid id);
	public Task<Receptionist> GetByOfficeIdAsync(Guid officeId);
	public Task<IEnumerable<Receptionist>> GetAllAsync();
}
