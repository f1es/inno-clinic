using AutoMapper;
using MediatR;
using Offices.Core.Dto.Response;
using Offices.Core.Models;
using Offices.Core.Repositories;
using Shared.Exceptions;

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
		var office = await _unitOfWork.OfficeRepository.GetByIdAsync(request.ReceptionistRequestDto.OfficeId);

		if (office == null)
		{
			throw new NotFoundException(nameof(office), request.ReceptionistRequestDto.OfficeId);
		}

		var existingReceptionist = await _unitOfWork.RecepcionistRepository.GetByOfficeIdAsync(office.Id);

		if (existingReceptionist != null)
		{
			throw new AlreadyExistException($"Receptionist in office with id {office.Id} already exist");
		} 

		var receptionist = _mapper.Map<Receptionist>(request.ReceptionistRequestDto);
		receptionist.Id = Guid.NewGuid();

		await _unitOfWork.RecepcionistRepository.CreateAsync(receptionist);

		var receptionistResponse = _mapper.Map<ReceptionistResponseDto>(receptionist);

		return receptionistResponse;
	}
}
