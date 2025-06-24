using AutoMapper;
using MediatR;
using Offices.Core.Cache;
using Offices.Core.Models;
using Offices.Core.Repositories;

namespace Offices.Application.CQRS.Offices.Commands.UpdateOffice;

public class UpdateOfficeCommandHandler : IRequestHandler<UpdateOfficeCommand>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IOfficesCacheService _officesCacheService;

	public UpdateOfficeCommandHandler(
		IMapper mapper,
		IUnitOfWork unitOfWork,
		IOfficesCacheService officesCacheService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_officesCacheService = officesCacheService;
	}

	public async Task Handle(UpdateOfficeCommand request, CancellationToken cancellationToken)
	{
		var newOffice = _mapper.Map<Office>(request.OfficeRequestDto);
		newOffice.Id = request.Id;

		await _unitOfWork.OfficeRepository.UpdateAsync(newOffice);

		await _officesCacheService.RemoveFromCacheAsync(request.Id, cancellationToken);
		await _officesCacheService.RemoveAllOfficesFromCacheAsync(cancellationToken);
	}
}
