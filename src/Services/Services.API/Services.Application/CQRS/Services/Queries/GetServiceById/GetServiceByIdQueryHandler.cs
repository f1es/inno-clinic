using MediatR;
using Services.Application.Mappers.Interfaces;
using Services.Core.Dto.Response;
using Services.Core.Repositories;
using Shared.Exceptions;

namespace Services.Application.CQRS.Services.Queries.GetServiceById;

public class GetServiceByIdQueryHandler : IRequestHandler<GetServiceByIdQuery, ServiceResponseDto>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IServicesMapper _servicesMapper;

	public GetServiceByIdQueryHandler(IUnitOfWork unitOfWork, IServicesMapper servicesMapper)
	{
		_unitOfWork = unitOfWork;
		_servicesMapper = servicesMapper;
	}

	public async Task<ServiceResponseDto> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
	{
		var service = await _unitOfWork.ServiceRepository.GetByIdAsync(request.Id);

		if (service == null)
		{
			throw new NotFoundException(nameof(service), request.Id);
		}

		return _servicesMapper.ToResponse(service);
	}
}
