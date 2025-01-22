namespace Services.Core.Models;

public class Service
{
	public Guid Id { get; set; }
	public string ServiceName { get; set; }
	public decimal Price { get; set; }
	public bool IsActive { get; set; }

	public Guid SpecializationId { get; set; }
}
