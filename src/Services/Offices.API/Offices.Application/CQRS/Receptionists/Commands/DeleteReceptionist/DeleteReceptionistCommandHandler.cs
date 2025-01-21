using AutoMapper;
using MediatR;
using Offices.Core.Repositories;
using Shared.Exceptions;

namespace Offices.Application.CQRS.Receptionists.Commands.DeleteREceptionist;

public class DeleteReceptionistCommandHandler : IRequestHandler<DeleteReceptionistCommand>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public DeleteReceptionistCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public async Task Handle(DeleteReceptionistCommand request, CancellationToken cancellationToken)
	{
		var receptionist = await _unitOfWork.RecepcionistRepository.GetByIdAsync(request.Id);

		if (receptionist == null)
		{
			throw new NotFoundException(nameof(receptionist), request.Id);
		}

		await _unitOfWork.RecepcionistRepository.DeleteAsync(receptionist);
	}
}
