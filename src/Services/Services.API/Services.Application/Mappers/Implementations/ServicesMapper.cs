using Riok.Mapperly.Abstractions;
using Services.Application.Dto.Request;
using Services.Application.Dto.Response;
using Services.Application.Mappers.Interfaces;
using Services.Core.Models;

namespace Services.Application.Mappers.Implementations;

[Mapper(AllowNullPropertyAssignment = false)]
public partial class ServicesMapper : IServicesMapper
{
	[MapperIgnoreTarget(nameof(Service.Id))]
	[MapperIgnoreTarget(nameof(Service.ServiceCategory))]
	public partial Service ToModel(ServiceRequestDto serviceRequestDto);
	[MapperIgnoreTarget(nameof(Service.Id))]
	[MapperIgnoreTarget(nameof(Service.ServiceCategory))]
	public partial void UpdateModel(ServiceRequestDto serviceRequestDto, Service service);
	[MapperIgnoreSource(nameof(Service.ServiceCategory))]
	public partial ServiceResponseDto ToResponse(Service service);
	[MapperIgnoreSource(nameof(Service.ServiceCategoryId))]
	public partial ServiceWithCategoryResponseDto ToResponseWithCategory(Service service);
	public partial IEnumerable<ServiceResponseDto> ToResponse(IEnumerable<Service> services);
}
