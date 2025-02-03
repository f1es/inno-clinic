using Profiles.Core.Models;
using Profiles.Core.Parameters;

namespace Profiles.Core.Repositories;

public interface IPatientRepository : IBaseReposirtory<Patient>
{
	public Task<Patient> GetByIdAsync(Guid id, bool trackChanges = false);
	public Task<IEnumerable<Patient>> GetAllAsync(RequestParameters requestParameters);
}
