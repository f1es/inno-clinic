namespace Authorization.Core.Models;

public class Account
{
	public Guid Id { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
	public string PhoneNumber { get; set; }
	public bool IsEmailVerified { get; set; }
	public string CreatedBy { get; set; }
	public DateTime CreatedAt { get; set; }
	public string UpdatedBy { get; set; }
	public DateTime UpdatedAt { get; set; }

	public Guid PhotoId { get; set; }
}
