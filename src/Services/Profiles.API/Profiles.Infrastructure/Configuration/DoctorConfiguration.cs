using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profiles.Core.Models;

namespace Profiles.Infrastructure.Configuration;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
	public void Configure(EntityTypeBuilder<Doctor> builder)
	{
		builder.HasKey(x => x.Id);

		builder.HasOne(x => x.Specialization)
			.WithMany(x => x.Doctors)
			.HasForeignKey(x => x.SpecializationId);
	}
}
