using MediatR;
using Services.Application.Mappers.Interfaces;
using Services.Core.Notifiers;
using Services.Core.Repositories;
using Shared.Exceptions;

namespace Services.Application.CQRS.Services.Commands.DeleteService;

public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IServicesMapper _servicesMapper;
	private readonly IMediator _mediator;
	private readonly IDeleteServiceNotifier _deleteServiceNotifier;

	public DeleteServiceCommandHandler(
		IUnitOfWork unitOfWork,
		IServicesMapper servicesMapper,
		IMediator mediator,
		IDeleteServiceNotifier deleteServiceNotifier)
	{
		_unitOfWork = unitOfWork;
		_servicesMapper = servicesMapper;
		_mediator = mediator;
		_deleteServiceNotifier = deleteServiceNotifier;
	}

	public async Task Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
	{
		var service = await _unitOfWork.ServiceRepository.GetByIdAsync(request.Id, cancellationToken);

		if (service == null)
		{
			throw new NotFoundException(nameof(service), request.Id);
		}

		_unitOfWork.ServiceRepository.Delete(service);

		await _unitOfWork.SaveAsync(cancellationToken);

		await _deleteServiceNotifier.NotifyAsync(service.Id, cancellationToken);
	}
}
