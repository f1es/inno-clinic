namespace Appointment.Core.Dto.Response;

public record ResultResponseDto(
	Guid Id,
	string Complaints,
	string Conclusion,
	string Reccomendations,
	Guid AppointmentId);
