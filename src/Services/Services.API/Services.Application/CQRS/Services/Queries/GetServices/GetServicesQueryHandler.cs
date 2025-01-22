using MediatR;
using Services.Application.Mappers.Interfaces;
using Services.Core.Dto.Response;
using Services.Core.Repositories;

namespace Services.Application.CQRS.Services.Queries.GetServices;

public class GetServicesQueryHandler : IRequestHandler<GetServicesQuery, IEnumerable<ServiceResponseDto>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IServicesMapper _servicesMapper;

	public GetServicesQueryHandler(IUnitOfWork unitOfWork, IServicesMapper servicesMapper)
	{
		_unitOfWork = unitOfWork;
		_servicesMapper = servicesMapper;
	}

	public async Task<IEnumerable<ServiceResponseDto>> Handle(GetServicesQuery request, CancellationToken cancellationToken)
	{
		var services = await _unitOfWork.ServiceRepository.GetAllAsync();

		return _servicesMapper.ToResponse(services);
	}
}
