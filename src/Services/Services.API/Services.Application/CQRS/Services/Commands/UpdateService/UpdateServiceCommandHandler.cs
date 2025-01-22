using MediatR;
using Services.Application.Mappers.Interfaces;
using Services.Core.Repositories;

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
		var service = await _unitOfWork.ServiceRepository.GetByIdAsync(request.Id, trackChanges: true);

		if (service == null)
		{
			// 404 ex
		}

		var newService = _servicesMapper.ToModel(request.ServiceRequestDto);
		service = newService;

		await _unitOfWork.SaveAsync();
	}
}
