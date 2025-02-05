using Riok.Mapperly.Abstractions;
using Services.Application.Mappers.Interfaces;
using Services.Core.Dto.Request;
using Services.Core.Dto.Response;
using Services.Core.Models;

namespace Services.Application.Mappers.Implementations;

[Mapper(AllowNullPropertyAssignment = false)]
public partial class ServiceCategoriesMapper : IServiceCategoriesMapper
{
	[MapperIgnoreTarget(nameof(ServiceCategory.Id))]
	[MapperIgnoreTarget(nameof(ServiceCategory.Services))]
	public partial ServiceCategory ToModel(ServiceCategoryRequestDto serviceCategoryRequestDto);
	[MapperIgnoreTarget(nameof(ServiceCategory.Id))]
	[MapperIgnoreTarget(nameof(ServiceCategory.Services))]
	public partial void UpdateModel(ServiceCategoryRequestDto serviceCategoryRequestDto, ServiceCategory serviceCategory);
	[MapperIgnoreSource(nameof(ServiceCategory.Services))]
	public partial ServiceCategoryResponseDto ToResponse(ServiceCategory serviceCategory);
	public partial IEnumerable<ServiceCategoryResponseDto> ToResponse(IEnumerable<ServiceCategory> serviceCategories);
}
