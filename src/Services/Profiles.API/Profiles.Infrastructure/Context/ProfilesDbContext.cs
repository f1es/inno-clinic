using Microsoft.EntityFrameworkCore;
using Profiles.Core.Models;
using System.Reflection;

namespace Profiles.Infrastructure.Context;

public class ProfilesDbContext : DbContext
{
	public DbSet<Doctor> Doctors { get; set; }
	public DbSet<Patient> Patients { get; set; }
	public DbSet<Receptionist> Receptionists { get; set; }
	public DbSet<Specialization> Specializations { get; set; }

    public ProfilesDbContext()
    { }

    public ProfilesDbContext(DbContextOptions<ProfilesDbContext> options) 
		: base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		base.OnModelCreating(modelBuilder);
	}
}
