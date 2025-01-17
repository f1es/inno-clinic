using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Offices.Core.Models;

namespace Offices.Infrastructure.Configuration;

public class ReceptionistsConfiguration : IEntityTypeConfiguration<Receptionist>
{
	public void Configure(EntityTypeBuilder<Receptionist> builder)
	{
		builder.HasKey(x => x.Id);

		builder.Property(x => x.FirstName)
			.IsRequired();

		builder.Property(x => x.LastName)
			.IsRequired();

		builder.Property(x => x.AccountId)
			.IsRequired();


		builder.HasOne(x => x.Office)
			.WithOne(x => x.Receptionist)
			.HasForeignKey<Receptionist>(x => x.OfficeId);
	}
}
