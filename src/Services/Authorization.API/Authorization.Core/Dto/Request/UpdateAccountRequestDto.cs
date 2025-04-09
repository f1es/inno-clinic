namespace Authorization.Core.Dto.Request;

public record UpdateAccountRequestDto(
	string PhoneNumber,
	Guid? PhotoId);
