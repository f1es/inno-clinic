using MediatR;
using Services.Application.CQRS.Notifier.Queries.DeleteServiceNotification;
using Services.Application.Mappers.Interfaces;
using Services.Core.Repositories;
using Shared.Exceptions;

namespace Services.Application.CQRS.Services.Commands.DeleteService;

public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IServicesMapper _servicesMapper;
	private readonly IMediator _mediator;

	public DeleteServiceCommandHandler(
		IUnitOfWork unitOfWork,
		IServicesMapper servicesMapper,
		IMediator mediator)
	{
		_unitOfWork = unitOfWork;
		_servicesMapper = servicesMapper;
		_mediator = mediator;
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

		var deleteServiceNotificationQuery = new DeleteServiceNotificationQuery(service.Id);
		await _mediator.Send(deleteServiceNotificationQuery);
	}
}
