using AutoMapper;
using MediatR;
using Offices.Core.Dto.Response;
using Offices.Core.Models;
using Offices.Core.Repositories;

namespace Offices.Application.CQRS.Offices.Commands.CreateOffice;

public class CreateOfficeCommandHandler : IRequestHandler<CreateOfficeCommand, OfficeResponseDto>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public CreateOfficeCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public async Task<OfficeResponseDto> Handle(CreateOfficeCommand request, CancellationToken cancellationToken)
	{
		var office = _mapper.Map<Office>(request.OfficeRequestDto);
		office.Id = Guid.NewGuid();

		await _unitOfWork.OfficeRepository.CreateAsync(office);

		var officeResponse = _mapper.Map<OfficeResponseDto>(office);

		return officeResponse;
	}
}
