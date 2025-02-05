using Profiles.Core.Models;

namespace Profiles.Infrastructure.Extensions;

public static class PatientRepositoryExtensions
{
	public static IQueryable<Patient> Search(this IQueryable<Patient> query, string searchTerm)
	{
		if (string.IsNullOrWhiteSpace(searchTerm))
		{
			return query;
		}

		return query.Where(x => 
		(x.FirstName + x.LastName + x.MiddleName + x.DateOfBirth).ToLower()
		.Contains(searchTerm.ToLower()));
	}
}
