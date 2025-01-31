using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profiles.Core.Models;

namespace Profiles.Infrastructure.Configuration;

public class SpecializationConfiguration : IEntityTypeConfiguration<Specialization>
{
	public void Configure(EntityTypeBuilder<Specialization> builder)
	{
		builder.HasKey(x => x.Id);
	}
}
