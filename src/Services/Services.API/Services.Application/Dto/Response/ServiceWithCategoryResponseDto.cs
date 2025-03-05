namespace Services.Application.Dto.Response;

public record ServiceWithCategoryResponseDto(
	Guid Id,
	string ServiceName,
	decimal Price,
	bool IsActive,
	Guid SpecializationId,
	ServiceCategoryResponseDto ServiceCategory);
