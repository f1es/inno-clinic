using Profiles.Core.Models;
using Profiles.Core.Parameters;
using Profiles.Core.Utility;

namespace Profiles.Core.Repositories;

public interface IReceptionistRepository : IBaseReposirtory<Receptionist>
{
	public Task<Receptionist> GetByIdAsync(Guid id, bool trackChanges = false);
	public Task<PagedList<Receptionist>> GetAllAsync(RequestParameters requestParameters);
}
