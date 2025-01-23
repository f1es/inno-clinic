using Riok.Mapperly.Abstractions;
using Services.Application.Mappers.Interfaces;
using Services.Core.Dto.Request;
using Services.Core.Dto.Response;
using Services.Core.Models;

namespace Services.Application.Mappers.Implementations;

[Mapper]
public partial class ServicesMapper : IServicesMapper
{
	[MapperIgnoreTarget(nameof(Service.Id))]
	[MapperIgnoreTarget(nameof(Service.ServiceCategory))]
	public partial Service ToModel(ServiceRequestDto serviceRequestDto);
	[MapperIgnoreSource(nameof(Service.ServiceCategory))]
	public partial ServiceResponseDto ToResponse(Service service);
	public partial IEnumerable<ServiceResponseDto> ToResponse(IEnumerable<Service> services);
}
