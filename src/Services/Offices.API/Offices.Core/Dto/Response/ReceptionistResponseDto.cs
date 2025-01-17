namespace Offices.Core.Dto.Response;

public record ReceptionistResponseDto(
	Guid Id,
	string FirstName,
	string LastName,
	string MiddleName,
	Guid AccountId,
	Guid OfficeId);
