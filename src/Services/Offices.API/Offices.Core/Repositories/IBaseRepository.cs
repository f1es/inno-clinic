namespace Offices.Core.Repositories;

public interface IBaseRepository<T> where T : class
{
	public Task CreateAsync(T entity);
	public Task UpdateAsync(T entity);
	public Task DeleteAsync(T entity);
}
