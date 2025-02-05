using Appointment.Core.Repositories;
using Appointment.Infrastructure.Context;

namespace Appointment.Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
	protected AppointmentDbContext _context;
    public BaseRepository(AppointmentDbContext context)
    {
        _context = context;
    }

    public void Create(T entity) => _context.Set<T>().Add(entity);

	public void Delete(T entity) => _context.Set<T>().Remove(entity);

	public void Update(T entity) => _context.Set<T>().Update(entity);
}
