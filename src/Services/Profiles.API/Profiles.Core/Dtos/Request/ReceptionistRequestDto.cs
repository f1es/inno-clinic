namespace Profiles.Core.Dtos.Request;

public record ReceptionistRequestDto(
	string FirstName,
	string LastName,
	string MiddleName,
	Guid AccountId,
	Guid OfficeId);
