using AutoMapper;
using MediatR;
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
		var receptionist = await _unitOfWork.RecepcionistRepository.GetByIdAsync(request.Id, trackChanges: true);

		if (receptionist == null)
		{
			// 404 Ex
		}

		receptionist = _mapper.Map(request.ReceptionistRequestDto, receptionist);

		await _unitOfWork.SaveAsync();
	}
}
