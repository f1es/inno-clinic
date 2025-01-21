namespace Offices.Core.Models;

public class Office
{
	public Guid Id { get; set; }
	public string Address { get; set; }
	public string RegistryPhoneNumber { get; set; }
	public bool IsActive { get; set; }

	public Guid PhotoId { get; set; }
}
