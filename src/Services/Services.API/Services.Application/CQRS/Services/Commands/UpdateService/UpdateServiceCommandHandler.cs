using MediatR;
using Services.Application.Mappers.Interfaces;
using Services.Core.Repositories;
using Shared.Exceptions;

namespace Services.Application.CQRS.Services.Commands.UpdateService;

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IServicesMapper _servicesMapper;

	public UpdateServiceCommandHandler(
		IUnitOfWork unitOfWork,
		IServicesMapper servicesMapper)
	{
		_unitOfWork = unitOfWork;
		_servicesMapper = servicesMapper;
	}

	public async Task Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
	{
		var service = await _unitOfWork.ServiceRepository.GetByIdAsync(request.Id, cancellationToken, trackChanges: true);

		if (service == null)
		{
			throw new NotFoundException(nameof(service), request.Id);
		}

		_servicesMapper.UpdateModel(request.ServiceRequestDto, service);

		await _unitOfWork.SaveAsync(cancellationToken);
	}
}
