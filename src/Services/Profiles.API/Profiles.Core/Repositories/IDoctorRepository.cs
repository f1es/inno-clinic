using Profiles.Core.Models;
using Profiles.Core.Parameters;
using Profiles.Core.Utility;

namespace Profiles.Core.Repositories;

public interface IDoctorRepository : IBaseReposirtory<Doctor>
{
	public Task<Doctor> GetByIdAsync(Guid id, bool trackChanges = false);
	public Task<PagedList<Doctor>> GetAllAsync(RequestParameters requestParameters);
}
