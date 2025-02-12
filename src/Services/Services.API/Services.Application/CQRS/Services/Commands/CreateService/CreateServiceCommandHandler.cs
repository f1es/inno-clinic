using MediatR;
using Services.Application.Mappers.Interfaces;
using Services.Core.Dto.Response;
using Services.Core.Repositories;

namespace Services.Application.CQRS.Services.Commands.CreateService;

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, ServiceResponseDto>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IServicesMapper _servicesMapper;

	public CreateServiceCommandHandler(
		IUnitOfWork unitOfWork,
		IServicesMapper servicesMapper)
	{
		_unitOfWork = unitOfWork;
		_servicesMapper = servicesMapper;
	}

	public async Task<ServiceResponseDto> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
	{
		var service = _servicesMapper.ToModel(request.ServiceRequestDto);

		_unitOfWork.ServiceRepository.Create(service);

		await _unitOfWork.SaveAsync(cancellationToken);

		return _servicesMapper.ToResponse(service);
	}
}
