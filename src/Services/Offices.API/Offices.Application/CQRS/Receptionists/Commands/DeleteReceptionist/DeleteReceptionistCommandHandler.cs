using AutoMapper;
using MediatR;
using Offices.Core.Repositories;

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
			// 404 Exception 
		}

		_unitOfWork.RecepcionistRepository.Delete(receptionist);

		await _unitOfWork.SaveAsync();
	}
}
