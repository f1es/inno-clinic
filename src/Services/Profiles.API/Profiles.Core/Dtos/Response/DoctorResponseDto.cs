namespace Profiles.Core.Dtos.Response;

public record DoctorResponseDto(
	Guid Id,
	string FirstName,
	string LastName,
	string MiddleName,
	DateOnly DateOfBirth,
	int CareerStartYear,
	string Status,
	Guid AccountId,
	Guid SpecializationId,
	Guid OfficeId);
