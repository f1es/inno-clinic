namespace Services.Application.Dto.Request;

public record ServiceRequestDto(
	string ServiceName, 
	decimal Price, 
	bool IsActive,
	Guid SpecializationId,
	Guid ServiceCategoryId);
