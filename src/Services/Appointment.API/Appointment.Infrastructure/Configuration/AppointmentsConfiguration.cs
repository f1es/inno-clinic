using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointment.Infrastructure.Configuration;

public class AppointmentsConfiguration : IEntityTypeConfiguration<Core.Models.Appointment>
{
	public void Configure(EntityTypeBuilder<Core.Models.Appointment> builder)
	{
		builder.HasKey(x => x.Id);
	}
}
