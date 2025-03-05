using Profiles.Core.Models;

namespace Profiles.Infrastructure.Extensions;

public static class SpecializationRepositoryExtensions
{
	public static IQueryable<Specialization> Search(this IQueryable<Specialization> query, string searchTerm)
	{
		if (string.IsNullOrWhiteSpace(searchTerm))
		{
			return query;
		}

		return query.Where(x =>
		x.SpecializationName.ToLower()
		.Contains(searchTerm.ToLower()));
	}
}
