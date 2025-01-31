using Profiles.Core.Repositories;
using Profiles.Infrastructure.Context;

namespace Profiles.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IBaseReposirtory<T> where T : class
{
	protected readonly ProfilesDbContext _context;

	protected BaseRepository(ProfilesDbContext context)
	{
		_context = context;
	}

	public void Create(T entity) => _context.Set<T>().Add(entity);

	public void Delete(T entity) => _context.Set<T>().Add(entity);

	public void Update(T entity) => _context.Set<T>().Add(entity);
}
