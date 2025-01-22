using Services.Core.Dto.Request;
using Services.Core.Dto.Response;
using Services.Core.Models;

namespace Services.Application.Mappers.Interfaces;

public interface IServiceCategoriesMapper
{
	public ServiceCategory ToModel(ServiceCategoryRequestDto serviceCategoryRequestDto);
	public ServiceCategoryResponseDto ToResponse(ServiceCategory serviceCategory);
}
