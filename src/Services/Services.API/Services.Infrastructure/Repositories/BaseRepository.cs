using Services.Core.Repositories;
using Services.Infrastructure.Context;

namespace Services.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
{
	protected ServicesDbContext _context;

	protected BaseRepository(ServicesDbContext context)
	{
		_context = context;
	}

	public void Create(T entity) => _context.Set<T>().Add(entity);

	public void Delete(T entity) => _context?.Set<T>().Remove(entity);

	public void Update(T entity) => _context.Set<T>().Update(entity);
}
