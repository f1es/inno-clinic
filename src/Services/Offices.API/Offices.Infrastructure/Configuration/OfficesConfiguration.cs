using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Offices.Core.Models;

namespace Offices.Infrastructure.Configuration;

public class OfficesConfiguration : IEntityTypeConfiguration<Office>
{
	public void Configure(EntityTypeBuilder<Office> builder)
	{
		builder.HasKey(x => x.Id);

		builder.Property(x => x.Address)
			.IsRequired();

		builder.Property(x => x.RegistryPhoneNumber)
			.IsRequired(); ;

		builder.Property(x => x.IsActive)
			.IsRequired();
	}
}
