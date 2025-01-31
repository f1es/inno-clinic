using Profiles.Core.Models;

namespace Profiles.Core.Repositories;

public interface IDoctorRepository : IBaseReposirtory<Doctor>
{
	public Task<Doctor> GetByIdAsync(Guid id, bool trackChanges = false);
	public Task<IEnumerable<Doctor>> GetAllAsync();
}
