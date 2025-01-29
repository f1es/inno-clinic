using Appointment.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointment.Infrastructure.Configuration;

public class ResultConfiguration : IEntityTypeConfiguration<Result>
{
	public void Configure(EntityTypeBuilder<Result> builder)
	{
		builder.HasKey(x => x.Id);

		builder.HasOne(x => x.Appointment)
			.WithOne(x => x.Result)
			.HasForeignKey<Result>(x => x.AppointmentId);
	}
}
