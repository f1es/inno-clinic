using Profiles.Core.Models;

namespace Profiles.Core.Repositories;

public interface ISpecializationRepository : IBaseReposirtory<Specialization>
{
	public Task<Specialization> GetByIdAsync(Guid id, bool trackChanges = false);
	public Task<IEnumerable<Specialization>> GetAllAsync();
}
