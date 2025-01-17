using Offices.Core.Repositories;
using Offices.Infrastructure.Context;

namespace Offices.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
{
	protected readonly OfficesDbContext _context;
    public BaseRepository(OfficesDbContext context)
    {
        _context = context;
    }

	public void Create(T entity)
	{
		_context.Set<T>().Add(entity);
	}

	public void Delete(T entity)
	{
		_context.Set<T>().Remove(entity);
	}

	public void Update(T entity)
	{
		_context.Set<T>().Update(entity);
	}
}
