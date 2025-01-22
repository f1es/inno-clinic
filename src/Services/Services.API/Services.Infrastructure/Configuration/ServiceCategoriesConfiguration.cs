using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Core.Models;

namespace Services.Infrastructure.Configuration;

public class ServiceCategoriesConfiguration : IEntityTypeConfiguration<ServiceCategory>
{
	public void Configure(EntityTypeBuilder<ServiceCategory> builder)
	{
		builder.HasKey(x => x.Id);

		builder.Property(x => x.CategoryName)
			.IsRequired();
	}
}
