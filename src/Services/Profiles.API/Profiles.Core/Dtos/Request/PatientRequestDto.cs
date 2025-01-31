namespace Profiles.Core.Dtos.Request;

public record PatientRequestDto(
	string FirstName,
	string LastName,
	string MiddleName,
	DateOnly DateOfBirth,
	Guid? AccountId);
