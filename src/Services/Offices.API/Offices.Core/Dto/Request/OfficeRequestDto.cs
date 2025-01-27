namespace Offices.Core.Dto.Request;

public record OfficeRequestDto(
	string Address, 
	string RegistryPhoneNumber, 
	bool IsActive,
	Guid PhotoId);

