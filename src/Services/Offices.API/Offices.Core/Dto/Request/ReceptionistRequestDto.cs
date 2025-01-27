namespace Offices.Core.Dto.Request;

public record ReceptionistRequestDto(
	string FirstName,
	string LastName,
	string MiddleName,
	Guid AccountId,
	Guid OfficeId);
