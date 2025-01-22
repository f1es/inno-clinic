namespace Services.Core.Dto.Request;

public record ServiceCategoryRequestDto(
	string CategoryName, 
	int TimeSlotSize);
