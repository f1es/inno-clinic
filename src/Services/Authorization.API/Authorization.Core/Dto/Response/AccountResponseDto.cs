namespace Authorization.Core.Dto.Response;

public record AccountResponseDto(
	Guid Id, 
	string Email,
	string PhoneNumber,
	bool IsEmailVerified,
	string CreatedBy,
	DateTime CreatedAt,
	string? UpdatedBy,
	DateTime? UpdatedAt,
	Guid PhotoId);
