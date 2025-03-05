namespace Profiles.Core.Models;

public class Doctor
{
	public Guid Id { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string MiddleName { get; set; }
	public DateOnly DateOfBirth { get; set; }
	public int CareerStartYear { get; set; }
	public string Status { get; set; }

	public Guid AccountId { get; set; }
	public Guid SpecializationId { get; set; }
	public Specialization Specialization { get; set; }
	public Guid OfficeId { get; set; }
}
