using Profiles.Core.Models;

namespace Profiles.Infrastructure.Extensions;

public static class ReceptionistRepositoryExtensions
{
	public static IQueryable<Receptionist> Search(this IQueryable<Receptionist> query, string searchTerm)
	{
		if (string.IsNullOrWhiteSpace(searchTerm))
		{
			return query;
		}

		return query.Where(x => (x.FirstName + x.LastName + x.MiddleName).ToLower()
		.Contains(searchTerm.ToLower()));
	}
}
