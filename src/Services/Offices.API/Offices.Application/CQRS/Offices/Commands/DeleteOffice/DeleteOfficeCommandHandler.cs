using MediatR;
using Offices.Core.Cache;
using Offices.Core.Repositories;
using Shared.Exceptions;

namespace Offices.Application.CQRS.Offices.Commands.DeleteOffice;

public class DeleteOfficeCommandHandler : IRequestHandler<DeleteOfficeCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IOfficesCacheService _officesCacheService;

	public DeleteOfficeCommandHandler(IUnitOfWork unitOfWork, IOfficesCacheService officesCacheService)
	{
		_unitOfWork = unitOfWork;
		_officesCacheService = officesCacheService;
	}

	public async Task Handle(DeleteOfficeCommand request, CancellationToken cancellationToken)
	{
		var office = await _unitOfWork.OfficeRepository.GetByIdAsync(request.Id);

		if (office == null)
		{
			throw new NotFoundException(nameof(office), request.Id);
		}

		await _unitOfWork.OfficeRepository.DeleteAsync(office);

		await _officesCacheService.RemoveFromCacheAsync(request.Id, cancellationToken);
		await _officesCacheService.RemoveAllOfficesFromCacheAsync(cancellationToken);
	}
}
