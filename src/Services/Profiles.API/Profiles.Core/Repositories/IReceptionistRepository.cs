using Profiles.Core.Models;
using Profiles.Core.Parameters;

namespace Profiles.Core.Repositories;

public interface IReceptionistRepository : IBaseReposirtory<Receptionist>
{
	public Task<Receptionist> GetByIdAsync(Guid id, bool trackChanges = false);
	public Task<IEnumerable<Receptionist>> GetAllAsync(RequestParameters requestParameters);
}
