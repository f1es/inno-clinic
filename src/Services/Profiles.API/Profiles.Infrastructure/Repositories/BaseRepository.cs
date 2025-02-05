using Profiles.Core.Repositories;
using Profiles.Infrastructure.Context;

namespace Profiles.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
{
	protected readonly ProfilesDbContext _context;

	protected BaseRepository(ProfilesDbContext context)
	{
		_context = context;
	}

	public void Create(T entity) => _context.Set<T>().Add(entity);

	public void Delete(T entity) => _context.Set<T>().Remove(entity);

	public void Update(T entity) => _context.Set<T>().Update(entity);
}
