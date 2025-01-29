using Appointment.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Appointment.Infrastructure.Context;

public class AppointmentDbContext : DbContext
{
	public DbSet<Core.Models.Appointment> Appointments { get; set; }
	public DbSet<Result> Results { get; set; }

    public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options)
    { }

    public AppointmentDbContext()
    { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}
}
