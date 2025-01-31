namespace Profiles.Core.Models;

public class Specialization
{
	public Guid Id { get; set; }
	public string SpecializationName { get; set; }
	public bool IsActive { get; set; }

	public IEnumerable<Doctor> Doctors { get; set; }
}
