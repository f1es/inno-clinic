using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Core.Models;

namespace Services.Infrastructure.Configuration;

public class ServicesConfiguration : IEntityTypeConfiguration<Service>
{
	public void Configure(EntityTypeBuilder<Service> builder)
	{
		builder.HasKey(x => x.Id);

		builder.Property(x => x.ServiceName)
			.IsRequired();

		builder.Property(x => x.Price)
			.IsRequired();

		builder.Property(x => x.IsActive)
			.IsRequired();
	}
}
