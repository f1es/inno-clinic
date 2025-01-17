using Microsoft.EntityFrameworkCore;
using Offices.Core.Models;
using System.Reflection;

namespace Offices.Infrastructure.Context;

public class OfficesDbContext : DbContext
{
	public DbSet<Office> Offices { get; set; }
	public DbSet<Receptionist> Receptionists { get; set; }

    public OfficesDbContext()
    { }

    public OfficesDbContext(DbContextOptions<OfficesDbContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}
}
