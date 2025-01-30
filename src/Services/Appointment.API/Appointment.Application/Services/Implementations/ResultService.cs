using Appointment.Application.Mappers.Interfaces;
using Appointment.Application.Services.Interfaces;
using Appointment.Core.Dto.Request;
using Appointment.Core.Dto.Response;
using Appointment.Core.Models;
using Appointment.Core.Repositories;
using Shared.Exceptions;

namespace Appointment.Application.Services.Implementations;

public class ResultService : IResultService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IResultsMapper	_resultsMapper;

	public ResultService(
		IUnitOfWork unitOfWork,
		IResultsMapper resultsMapper)
	{
		_unitOfWork = unitOfWork;
		_resultsMapper = resultsMapper;
	}

	public async Task<ResultResponseDto> CreateAsync(ResultRequestDto resultRequestDto)
	{
		var result = _resultsMapper.ToModel(resultRequestDto);

		_unitOfWork.ResultRepository.Create(result);

		await _unitOfWork.SaveAsync();

		return _resultsMapper.ToResponse(result);
	}

	public async Task DeleteAsync(Guid id)
	{
		var result = await GetByIdAsyncAndCheckIfExist(id);

		_unitOfWork.ResultRepository.Delete(result);

		await _unitOfWork.SaveAsync();
	}

	public async Task<IEnumerable<ResultResponseDto>> GetAllAsync()
	{
		var results = await _unitOfWork.ResultRepository.GetAllAsync();

		return _resultsMapper.ToResponse(results);
	}

	public async Task<ResultResponseDto> GetByIdAsync(Guid id)
	{
		var result = await GetByIdAsyncAndCheckIfExist(id);

		return _resultsMapper.ToResponse(result);
	}

	public async Task<ResultForDownloadResponseDto> GetForDownloadAsync(Guid id)
	{
		var result = await GetByIdAsyncAndCheckIfExist(id);

		return _resultsMapper.ToResponseForDownload(result);
	}

	public async Task UpdateAsync(Guid id, ResultRequestDto resultRequestDto)
	{
		var result = await GetByIdAsyncAndCheckIfExist(id, trackChanges: true);

		result.AppointmentId = resultRequestDto.AppointmentId;
		result.Conclusion = resultRequestDto.Conclusion;
		result.Reccomendations = resultRequestDto.Reccomendations;
		result.Complaints = resultRequestDto.Complaints;

		await _unitOfWork.SaveAsync();
	}

	private async Task<Result> GetByIdAsyncAndCheckIfExist(Guid id, bool trackChanges = false)
	{
		var result = await _unitOfWork.ResultRepository.GetByIdAsync(id, trackChanges);

		return result ?? throw new NotFoundException(nameof(result), id);
	}
}
