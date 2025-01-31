using Profiles.Core.Models;

namespace Profiles.Core.Repositories;

public interface IPatientRepository : IBaseReposirtory<Patient>
{
	public Task<Patient> GetByIdAsync(Guid id, bool trackChanges = false);
	public Task<IEnumerable<Patient>> GetAllAsync();
}
