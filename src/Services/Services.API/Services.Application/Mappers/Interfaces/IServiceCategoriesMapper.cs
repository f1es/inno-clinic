using Services.Application.Dto.Request;
using Services.Application.Dto.Response;
using Services.Core.Models;

namespace Services.Application.Mappers.Interfaces;

public interface IServiceCategoriesMapper
{
	public ServiceCategory ToModel(ServiceCategoryRequestDto serviceCategoryRequestDto);
	public void UpdateModel(ServiceCategoryRequestDto serviceCategoryRequestDto, ServiceCategory serviceCategory);
	public ServiceCategoryResponseDto ToResponse(ServiceCategory serviceCategory);
	public IEnumerable<ServiceCategoryResponseDto> ToResponse(IEnumerable<ServiceCategory> serviceCategories);
}
