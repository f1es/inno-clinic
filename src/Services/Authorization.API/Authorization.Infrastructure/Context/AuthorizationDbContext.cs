using Authorization.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Authorization.Infrastructure.Context;

public class AuthorizationDbContext : DbContext
{
	public DbSet<Account> Accounts { get; set; }

    public AuthorizationDbContext()
    { }

    public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options)
        : base(options)
    { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}
}
