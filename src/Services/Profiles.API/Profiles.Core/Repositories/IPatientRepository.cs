using Profiles.Core.Models;
using Profiles.Core.Parameters;
using Profiles.Core.Utility;

namespace Profiles.Core.Repositories;

public interface IPatientRepository : IBaseReposirtory<Patient>
{
	public Task<Patient> GetByIdAsync(Guid id, bool trackChanges = false);
	public Task<PagedList<Patient>> GetAllAsync(RequestParameters requestParameters);
}
