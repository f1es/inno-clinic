using Profiles.Core.Models;

namespace Profiles.Infrastructure.Extensions;

public static class DoctorRepositoryExtensions
{
	public static IQueryable<Doctor> Search(this IQueryable<Doctor> doctors, string searchTerm)
	{
		if (string.IsNullOrWhiteSpace(searchTerm))
		{
			return doctors;
		}

		return doctors.Where(x =>
		(x.FirstName + x.LastName + x.MiddleName + x.Status).ToLower()
		.Contains(searchTerm.ToLower()));
	}
}
