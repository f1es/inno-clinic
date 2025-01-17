namespace Offices.Core.Dto.Request;

public record class ReceptionistRequestDto(
	string FirstName,
	string LastName,
	string MiddleName,
	Guid AccountId,
	Guid OfficeId);
