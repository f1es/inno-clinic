using Authorization.Core.Models;
using Authorization.Core.Repositories;
using Authorization.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
	private readonly AuthorizationDbContext _context;

	public AccountRepository(AuthorizationDbContext context)
	{
		_context = context;
	}

	public void Create(Account account) => _context.Accounts.Add(account);
	public void Delete(Account account) => _context.Accounts.Remove(account);
	public void Update(Account account) => _context.Accounts.Update(account);
	public async Task<IEnumerable<Account>> GetAllAsync() => await _context.Accounts.ToListAsync();
	public async Task<Account> GetByIdAsync(Guid id, bool trackChanges = false)
	{
		var query = trackChanges ? _context.Accounts : _context.Accounts.AsNoTracking();
		return await query.FirstOrDefaultAsync(x => x.Id == id);
	}
	public async Task<Account> GetByEmailAsync(string email, bool trackChanges = false)
	{
		var query = trackChanges ? _context.Accounts : _context.Accounts.AsNoTracking();
		return await query.FirstOrDefaultAsync(x => x.Email == email);
	}
	public async Task SaveAsync() => await _context.SaveChangesAsync();
}
