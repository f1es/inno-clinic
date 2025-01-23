using Authorization.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.Infrastructure.Configuration;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
	public void Configure(EntityTypeBuilder<Account> builder)
	{
		builder.HasKey(x => x.Id);

		builder.Property(x => x.Email).IsRequired();

		builder.Property(x => x.Password).IsRequired();

		builder.Property(x => x.CreatedAt).IsRequired();
	}
}
