namespace Offices.Core.Repositories;

public interface IBaseRepository<T> where T : class
{
	public Task CreateAsync(T entity, CancellationToken cancellationToken = default);
	public Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
	public Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}
