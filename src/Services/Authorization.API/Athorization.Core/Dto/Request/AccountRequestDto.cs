namespace Authorization.Core.Dto.Request;

public record AccountRequestDto(
	string Email,
	string Password,
	string PhoneNumber);
