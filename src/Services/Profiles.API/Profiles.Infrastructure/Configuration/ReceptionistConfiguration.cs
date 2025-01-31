using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profiles.Core.Models;

namespace Profiles.Infrastructure.Configuration;

public class ReceptionistConfiguration : IEntityTypeConfiguration<Receptionist>
{
	public void Configure(EntityTypeBuilder<Receptionist> builder)
	{
		builder.HasKey(x => x.Id);
	}
}
