using Profiles.Core.Models;
using Profiles.Core.Parameters;
using Profiles.Core.Utility;

namespace Profiles.Core.Repositories;

public interface ISpecializationRepository : IBaseRepository<Specialization>
{
	public Task<Specialization> GetByIdAsync(Guid id, bool trackChanges = false);
	public Task<PagedList<Specialization>> GetAllAsync(RequestParameters requestParameters);
}
