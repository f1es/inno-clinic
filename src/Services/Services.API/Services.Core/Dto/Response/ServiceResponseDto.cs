namespace Services.Core.Dto.Response;

public record ServiceResponseDto(
	Guid Id,
	string ServiceName,
	decimal Price,
	bool IsActive,
	Guid SpecializationId);
