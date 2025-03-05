using Microsoft.EntityFrameworkCore;
using Services.Core.Models;
using System.Reflection;

namespace Services.Infrastructure.Context;

public class ServicesDbContext : DbContext
{
	public DbSet<Service>? Services { get; set; }
	public DbSet<ServiceCategory>? ServiceCategories { get; set; }

    public ServicesDbContext()
    { }

    public ServicesDbContext(DbContextOptions<ServicesDbContext> options)
        : base(options)
    {
        
    }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}
}
