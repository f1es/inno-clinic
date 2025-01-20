using MediatR;
using Offices.Core.Repositories;
using Shared.Exceptions;

namespace Offices.Application.CQRS.Offices.Commands.DeleteOffice;

public class DeleteOfficeCommandHandler : IRequestHandler<DeleteOfficeCommand>
{
	private readonly IUnitOfWork _unitOfWork;

	public DeleteOfficeCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task Handle(DeleteOfficeCommand request, CancellationToken cancellationToken)
	{
		var office = await _unitOfWork.OfficeRepository.GetByIdAsync(request.Id);

		if (office == null)
		{
			throw new NotFoundException(nameof(office), request.Id);
		}

		_unitOfWork.OfficeRepository.Delete(office);

		await _unitOfWork.SaveAsync();
	}
}
