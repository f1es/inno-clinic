using Services.Core.Dto.Request;
using Services.Core.Dto.Response;
using Services.Core.Models;

namespace Services.Application.Mappers.Interfaces;

public interface IServicesMapper
{
	public Service ToModel(ServiceRequestDto serviceRequestDto);
	public ServiceResponseDto ToResponse(Service service);
}
