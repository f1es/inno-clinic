namespace Authorization.Core.Dto.Request;

public record LoginAccountRequestDto(
	string Email,
	string Password);
