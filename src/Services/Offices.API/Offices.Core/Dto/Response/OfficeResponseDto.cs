namespace Offices.Core.Dto.Response;

public record OfficeResponseDto(
	Guid Id,
	string Address,
	string RegistryPhoneNumber,
	bool IsActive,
	Guid PhotoId);
