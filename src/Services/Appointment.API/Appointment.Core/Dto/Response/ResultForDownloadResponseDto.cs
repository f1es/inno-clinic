namespace Appointment.Core.Dto.Response;

public record ResultForDownloadResponseDto(
	string Complaints,
	string Conclusion,
	string Reccomendations);
