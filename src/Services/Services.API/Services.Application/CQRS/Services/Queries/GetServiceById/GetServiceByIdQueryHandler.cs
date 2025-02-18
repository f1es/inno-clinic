using MediatR;
using Services.Application.Dto.Response;
using Services.Application.Mappers.Interfaces;
using Services.Core.Repositories;
using Shared.Exceptions;

namespace Services.Application.CQRS.Services.Queries.GetServiceById;

public class GetServiceByIdQueryHandler : IRequestHandler<GetServiceByIdQuery, ServiceWithCategoryResponseDto>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IServicesMapper _servicesMapper;

	public GetServiceByIdQueryHandler(IUnitOfWork unitOfWork, IServicesMapper servicesMapper)
	{
		_unitOfWork = unitOfWork;
		_servicesMapper = servicesMapper;
	}

	public async Task<ServiceWithCategoryResponseDto> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
	{
		var service = await _unitOfWork.ServiceRepository.GetByIdWithCategoryAsync(request.Id, cancellationToken);

		if (service == null)
		{
			throw new NotFoundException(nameof(service), request.Id);
		}

		return _servicesMapper.ToResponseWithCategory(service);
	}
}
