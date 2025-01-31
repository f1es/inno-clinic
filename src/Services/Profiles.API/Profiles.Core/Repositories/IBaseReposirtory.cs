namespace Profiles.Core.Repositories;

public interface IBaseReposirtory<T> where T : class
{
	public void Create(T entity);
	public void Update(T entity);
	public void Delete(T entity);
}
