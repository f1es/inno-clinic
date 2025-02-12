namespace Services.Application.Dto.Request;

public record ServiceCategoryRequestDto(
	string CategoryName, 
	int TimeSlotSize);
