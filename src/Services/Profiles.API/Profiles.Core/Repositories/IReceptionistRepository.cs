using Profiles.Core.Models;

namespace Profiles.Core.Repositories;

public interface IReceptionistRepository
{
	public Task<Receptionist> GetByIdAsync(Guid id, bool trackChanges = false);
	public Task<IEnumerable<Receptionist>> GetAllAsync();
}
