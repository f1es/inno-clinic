namespace Profiles.Core.Dtos.Response;

public record SpecializationResponseDto(
	Guid Id,
	string SpecializationName,
	bool IsActive);
