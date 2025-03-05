using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profiles.Core.Models;

namespace Profiles.Infrastructure.Configuration;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
	public void Configure(EntityTypeBuilder<Patient> builder)
	{
		builder.HasKey(x => x.Id);
	}
}
