using Authorization.Core.Models;

namespace Authorization.Core.Repositories;

public interface IAccountRepository
{
	public void Create(Account account);
	public void Update(Account account);
	public void Delete(Account account);
	public Task<Account> GetByIdAsync(Guid id);
	public Task<IEnumerable<Account>> GetAllAsync();
}
