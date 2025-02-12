namespace Services.Application.Dto.Response;

public record ServiceCategoryResponseDto(
	Guid Id,
	string CategoryName,
	int TimeSlotSize);
