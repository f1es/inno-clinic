namespace Services.Core.Dto.Response;

public record ServiceCategoryResponseDto(
	Guid Id,
	string CategoryName,
	int TimeSlotSize);
