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

public class SpecializationService : ISpecializationService
{
	private readonly IUnitOfWork _unitOfWork;

	public SpecializationService(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<SpecializationResponseDto> CreateAsync(SpecializationRequestDto specializationRequestDto)
	{
		var specialization = specializationRequestDto.Adapt<Specialization>();

		_unitOfWork.SpecializationRepository.Create(specialization);

		await _unitOfWork.SaveAsync();

		return specialization.Adapt<SpecializationResponseDto>();
	}

	public async Task DeleteAsync(Guid id)
	{
		var specialization = await _unitOfWork.SpecializationRepository.GetByIdAsync(id);

		SpecializationNullCheck(specialization, id);

		_unitOfWork.SpecializationRepository.Delete(specialization);

		await _unitOfWork.SaveAsync();
	}

	public async Task<PagedList<SpecializationResponseDto>> GetAllAsync(RequestParameters requestParameters)
	{
		var specializations = await _unitOfWork.SpecializationRepository.GetAllAsync(requestParameters);

		return specializations.Adapt<IEnumerable<SpecializationResponseDto>>()
			.Paginate(requestParameters.Page, requestParameters.PageSize);
	}

	public async Task<SpecializationResponseDto> GetByIdAsync(Guid id)
	{
		var specialization = await _unitOfWork.SpecializationRepository.GetByIdAsync(id);

		SpecializationNullCheck(specialization, id);

		return specialization.Adapt<SpecializationResponseDto>();
	}

	public async Task UpdateAsync(Guid id, SpecializationRequestDto specializationRequestDto)
	{
		var specialization = await _unitOfWork.SpecializationRepository.GetByIdAsync(id, trackChanges: true);

		SpecializationNullCheck(specialization, id);

		specializationRequestDto.Adapt(specialization);

		await _unitOfWork.SaveAsync();
	}

	private Specialization SpecializationNullCheck(Specialization specialization, Guid id) => specialization ?? throw new NotFoundException(nameof(specialization), id);
}
