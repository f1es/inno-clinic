using AutoMapper;
using MediatR;
using Offices.Core.Dto.Response;
using Offices.Core.Models;
using Offices.Core.Repositories;

namespace Offices.Application.CQRS.Receptionists.Commands.CreateRecepcionist;

public class CreateReceptionistCommandHandler : IRequestHandler<CreateReceptionistCommand, ReceptionistResponseDto>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public CreateReceptionistCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public async Task<ReceptionistResponseDto> Handle(CreateReceptionistCommand request, CancellationToken cancellationToken)
	{
		//var company

		var receptionist = _mapper.Map<Receptionist>(request.ReceptionistRequestDto);

		_unitOfWork.RecepcionistRepository.Create(receptionist);

		await _unitOfWork.SaveAsync();

		var receptionistResponse = _mapper.Map<ReceptionistResponseDto>(receptionist);

		return receptionistResponse;
	}
}
