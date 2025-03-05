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

	public async Task<ResultResponseDto> CreateAsync(ResultRequestDto resultRequestDto, CancellationToken cancellationToken)
	{
		var result = _resultsMapper.ToModel(resultRequestDto);

		_unitOfWork.ResultRepository.Create(result);
		await _unitOfWork.SaveAsync(cancellationToken);

		return _resultsMapper.ToResponse(result);
	}

	public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
	{
		var result = await GetByIdAsyncAndCheckIfExist(id, cancellationToken);

		_unitOfWork.ResultRepository.Delete(result);
		await _unitOfWork.SaveAsync(cancellationToken);
	}

	public async Task<IEnumerable<ResultResponseDto>> GetAllAsync(CancellationToken cancellationToken)
	{
		var results = await _unitOfWork.ResultRepository.GetAllAsync(cancellationToken);

		return _resultsMapper.ToResponse(results);
	}

	public async Task<ResultResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		var result = await GetByIdAsyncAndCheckIfExist(id, cancellationToken);

		return _resultsMapper.ToResponse(result);
	}

	public async Task<ResultForDownloadResponseDto> GetForDownloadAsync(Guid id, CancellationToken cancellationToken)
	{
		var result = await GetByIdAsyncAndCheckIfExist(id, cancellationToken);

		return _resultsMapper.ToResponseForDownload(result);
	}

	public async Task UpdateAsync(Guid id, ResultRequestDto resultRequestDto, CancellationToken cancellationToken)
	{
		var result = await GetByIdAsyncAndCheckIfExist(id, cancellationToken, trackChanges: true);

		_resultsMapper.Update(resultRequestDto, result);

		await _unitOfWork.SaveAsync(cancellationToken);
	}

	private async Task<Result> GetByIdAsyncAndCheckIfExist(Guid id, CancellationToken cancellationToken, bool trackChanges = false)
	{
		var result = await _unitOfWork.ResultRepository.GetByIdAsync(id, cancellationToken, trackChanges);

		return result ?? throw new NotFoundException(nameof(result), id);
	}
}
