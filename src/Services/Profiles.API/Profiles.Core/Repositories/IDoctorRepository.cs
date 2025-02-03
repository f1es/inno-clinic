using Profiles.Core.Models;
using Profiles.Core.Parameters;

namespace Profiles.Core.Repositories;

public interface IDoctorRepository : IBaseReposirtory<Doctor>
{
	public Task<Doctor> GetByIdAsync(Guid id, bool trackChanges = false);
	public Task<IEnumerable<Doctor>> GetAllAsync(RequestParameters requestParameters);
}
