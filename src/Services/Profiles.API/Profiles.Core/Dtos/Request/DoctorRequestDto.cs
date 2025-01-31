namespace Profiles.Core.Dtos.Request;

public record DoctorRequestDto(
	string FirstName,
	string LastName,
	string MiddleName,
	DateOnly DateOfBirth,
	int CareerStartYear,
	string Status,
	Guid AccountId,
	Guid SpecializationId,
	Guid OfficeId);
