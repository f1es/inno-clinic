using AutoMapper;
using MediatR;
using Offices.Core.Models;
using Offices.Core.Repositories;

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
		var newReceptionist = _mapper.Map<Receptionist>(request.ReceptionistRequestDto);
		newReceptionist.Id = request.Id;

		await _unitOfWork.RecepcionistRepository.UpdateAsync(newReceptionist);
	}
}
