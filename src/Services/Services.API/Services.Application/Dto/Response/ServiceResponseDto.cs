namespace Services.Application.Dto.Response;

public record ServiceResponseDto(
	Guid Id,
	string ServiceName,
	decimal Price,
	bool IsActive,
	Guid SpecializationId,
	Guid ServiceCategoryId);
