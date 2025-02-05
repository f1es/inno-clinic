namespace Appointment.Core.Dto.Request;

public record ResultRequestDto(
	string Complaints,
	string Conclusion,
	string Reccomendations,
	Guid AppointmentId);
