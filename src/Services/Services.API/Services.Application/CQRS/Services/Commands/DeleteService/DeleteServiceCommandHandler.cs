using MediatR;
using Services.Application.Mappers.Interfaces;
using Services.Core.Repositories;

namespace Services.Application.CQRS.Services.Commands.DeleteService;

public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IServicesMapper _servicesMapper;

	public DeleteServiceCommandHandler(
		IUnitOfWork unitOfWork, 
		IServicesMapper servicesMapper)
	{
		_unitOfWork = unitOfWork;
		_servicesMapper = servicesMapper;
	}

	public async Task Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
	{
		var service = await _unitOfWork.ServiceRepository.GetByIdAsync(request.Id);

		if (service == null)
		{
			// 404 ex
		}

		_unitOfWork.ServiceRepository.Delete(service);

		await _unitOfWork.SaveAsync();
	}
}
