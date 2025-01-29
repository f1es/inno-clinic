namespace Appointment.Core.Models;

public class Result
{
	public Guid Id { get; set; }
	public string? Complaints { get; set; }
	public string? Conclusion { get; set; }
	public string? Reccomendations { get; set; }

	public Guid AppointmentId { get; set; }
	public Appointment Appointment { get; set; }
}
