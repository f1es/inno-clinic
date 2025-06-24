using AutoMapper;
using MediatR;
using Offices.Core.Cache;
using Offices.Core.Dto.Response;
using Offices.Core.Models;
using Offices.Core.Repositories;

namespace Offices.Application.CQRS.Offices.Commands.CreateOffice;

public class CreateOfficeCommandHandler : IRequestHandler<CreateOfficeCommand, OfficeResponseDto>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IOfficesCacheService _officesCacheService;

	public CreateOfficeCommandHandler(
		IMapper mapper,
		IUnitOfWork unitOfWork,
		IOfficesCacheService officesCacheService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_officesCacheService = officesCacheService;
	}

	public async Task<OfficeResponseDto> Handle(CreateOfficeCommand request, CancellationToken cancellationToken)
	{
		var office = _mapper.Map<Office>(request.OfficeRequestDto);

		await _unitOfWork.OfficeRepository.CreateAsync(office);

		await _officesCacheService.RemoveAllOfficesFromCacheAsync(cancellationToken);

		return _mapper.Map<OfficeResponseDto>(office);
	}
}
