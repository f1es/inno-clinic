namespace Appointment.Core.Dto.Response;

public record PatientResponseDto(
	Guid Id,
	string FirstName,
	string LastName,
	string MiddleName,
	bool IsLinkedToAccount,
	DateOnly DateOfBirth,
	Guid? AccountId);