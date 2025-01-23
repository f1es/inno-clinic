namespace Services.Core.Dto.Request;

public record ServiceRequestDto(
	string ServiceName, 
	decimal Price, 
	bool IsActive,
	Guid SpecializationId,
	Guid ServiceCategoryId);
