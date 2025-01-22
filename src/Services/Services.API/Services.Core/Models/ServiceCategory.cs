namespace Services.Core.Models;

public class ServiceCategory
{
	public Guid Id { get; set; }
	public string CategoryName { get; set; }
	public int TimeSlotSize { get; set; }
}
