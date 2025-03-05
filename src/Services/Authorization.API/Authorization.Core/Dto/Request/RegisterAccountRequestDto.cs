namespace Authorization.Core.Dto.Request;

public record RegisterAccountRequestDto(
	string Email,
	string Password,
	string PhoneNumber,
	Guid PhotoId);