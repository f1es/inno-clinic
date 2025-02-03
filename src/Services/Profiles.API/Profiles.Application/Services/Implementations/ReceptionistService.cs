using Mapster;
using Profiles.Application.Extensions;
using Profiles.Application.Services.Interfaces;
using Profiles.Application.Utility;
using Profiles.Core.Dtos.Request;
using Profiles.Core.Dtos.Response;
using Profiles.Core.Models;
using Profiles.Core.Parameters;
using Profiles.Core.Repositories;
using Shared.Exceptions;

namespace Profiles.Application.Services.Implementations;

public class ReceptionistService : IReceptionistService
{
	private readonly IUnitOfWork _unitOfWork;

	public ReceptionistService(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<ReceptionistResponseDto> CreateAsync(ReceptionistRequestDto receptionistRequestDto)
	{
		var receptionist = receptionistRequestDto.Adapt<Receptionist>();

		_unitOfWork.ReceptionistRepository.Create(receptionist);

		await _unitOfWork.SaveAsync();

		return receptionist.Adapt<ReceptionistResponseDto>();
	}

	public async Task DeleteAsync(Guid id)
	{
		var receptionist = await _unitOfWork.ReceptionistRepository.GetByIdAsync(id);

		ReceptionistNullCheck(receptionist, id);

		_unitOfWork.ReceptionistRepository.Delete(receptionist);

		await _unitOfWork.SaveAsync();
	}

	public async Task<PagedList<ReceptionistResponseDto>> GetAllAsync(RequestParameters requestParameters)
	{
		var receptionists = await _unitOfWork.ReceptionistRepository.GetAllAsync(requestParameters);

		return receptionists.Adapt<IEnumerable<ReceptionistResponseDto>>()
			.Paginate(requestParameters.Page, requestParameters.PageSize);
	}

	public async Task<ReceptionistResponseDto> GetByIdAsync(Guid id)
	{
		var receptionist = await _unitOfWork.ReceptionistRepository.GetByIdAsync(id);

		ReceptionistNullCheck(receptionist, id);

		return receptionist.Adapt<ReceptionistResponseDto>();
	}

	public async Task UpdateAsync(Guid id, ReceptionistRequestDto receptionistRequestDto)
	{
		var receptionist = await _unitOfWork.ReceptionistRepository.GetByIdAsync(id, trackChanges: true);

		ReceptionistNullCheck(receptionist, id);

		receptionistRequestDto.Adapt(receptionist);

		await _unitOfWork.SaveAsync();
	}

	private Receptionist ReceptionistNullCheck(Receptionist receptionist, Guid id) => receptionist ?? throw new NotFoundException(nameof(receptionist), id); 
}
