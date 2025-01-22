using Riok.Mapperly.Abstractions;
using Services.Application.Mappers.Interfaces;
using Services.Core.Dto.Request;
using Services.Core.Dto.Response;
using Services.Core.Models;

namespace Services.Application.Mappers.Implementations;

[Mapper]
public partial class ServiceCategoriesMapper : IServiceCategoriesMapper
{
	[MapperIgnoreTarget(nameof(ServiceCategory.Id))]
	public partial ServiceCategory ToModel(ServiceCategoryRequestDto serviceCategoryRequestDto);
	public partial ServiceCategoryResponseDto ToResponse(ServiceCategory serviceCategory);
}
