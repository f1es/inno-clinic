namespace Authorization.Core.Dto.Response;

public record AccountResponseDto(
	Guid Id,
	string? FirstName,
	string? LastName,
	string? MiddleName,
	string Email,
	string PhoneNumber,
	bool IsEmailVerified,
	string CreatedBy,
	DateTime CreatedAt,
	string? UpdatedBy,
	DateTime? UpdatedAt,
	string? Role,
	Guid? PhotoId);
