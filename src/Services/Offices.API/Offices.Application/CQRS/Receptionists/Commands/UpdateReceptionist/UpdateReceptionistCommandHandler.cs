using AutoMapper;
using MediatR;
using Offices.Core.Repositories;
using Shared.Exceptions;

namespace Offices.Application.CQRS.Receptionists.Commands.UpdateReceptionist;

public class UpdateReceptionistCommandHandler : IRequestHandler<UpdateReceptionistCommand>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public UpdateReceptionistCommandHandler(
		IMapper mapper,
		IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public async Task Handle(UpdateReceptionistCommand request, CancellationToken cancellationToken)
	{
		var receptionist = await _unitOfWork.RecepcionistRepository.GetByIdAsync(request.Id, trackChanges: true);

		if (receptionist == null)
		{
			throw new NotFoundException(nameof(receptionist), request.Id);
		}

		receptionist = _mapper.Map(request.ReceptionistRequestDto, receptionist);

		await _unitOfWork.SaveAsync();
	}
}
