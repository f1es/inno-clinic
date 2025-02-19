using Appointment.Application.Mappers.Implementations;
using Appointment.Application.Mappers.Interfaces;
using Appointment.Application.Services.Implementations;
using Appointment.Core.Dto.Request;
using Appointment.Core.Models;
using Appointment.Core.Repositories;
using AutoFixture;
using Moq;
using Shared.Exceptions;

namespace Appointment.Tests.Services;

public class ResultServiceTests
{
	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly IResultsMapper _resultsMapper;
	private readonly Fixture _fixture;

	private readonly ResultService _resultService;

    public ResultServiceTests()
    {
		_unitOfWorkMock = new Mock<IUnitOfWork>();
		_resultsMapper = new ResultsMapper();
		_fixture = new Fixture();

		_resultService = new ResultService(
			_unitOfWorkMock.Object,
			_resultsMapper);
	}

	[Fact]
	public async Task CreateAsync_ValidObjectRequest_ReturnsResultResponseDto()
	{
		// Arrange
		var resultRequest = _fixture.Create<ResultRequestDto>();
		var resultModel = _resultsMapper.ToModel(resultRequest);
		var resultResponse = _resultsMapper.ToResponse(resultModel);

		_unitOfWorkMock.Setup(x => x.ResultRepository.Create(It.IsAny<Result>()));

		// Act
		var actResult = await _resultService.CreateAsync(resultRequest);

		// Assert
		_unitOfWorkMock.Verify(x => x.ResultRepository.Create(It.IsAny<Result>()), Times.Once);

		Assert.Equivalent(resultResponse, actResult);
	}

	[Fact]
	public async Task DeleteAsync_ResultId_ReturnsTask()
	{
		// Arrange
		var result = _fixture.Build<Result>().Without(x => x.Appointment).Create();
		var id = Guid.NewGuid();
		var trackChanges = false;

		_unitOfWorkMock.Setup(x => x.ResultRepository.GetByIdAsync(id, trackChanges)).
			ReturnsAsync(result);
		_unitOfWorkMock.Setup(x => x.ResultRepository.Delete(result));

		// Act
		await _resultService.DeleteAsync(id);

		// Assert
		_unitOfWorkMock.Verify(x => x.ResultRepository.GetByIdAsync(id, trackChanges), Times.Once);
		_unitOfWorkMock.Verify(x => x.ResultRepository.Delete(result), Times.Once);
	}

	[Fact]
	public async Task DeleteAsync_InvalidResultId_ThrowsNotFoundException()
	{
		// Arrange
		var id = new Guid();
		var trackChanges = false;
		var result = _fixture.Build<Result>().Without(x => x.Appointment).Create();

		_unitOfWorkMock.Setup(x => x.ResultRepository.GetByIdAsync(id, trackChanges)).
			ReturnsAsync(It.IsAny<Result>());
		_unitOfWorkMock.Setup(x => x.ResultRepository.Delete(result));

		// Act
		var function = async () => await _resultService.DeleteAsync(id);

		// Assert
		await Assert.ThrowsAsync<NotFoundException>(function);
	}

	[Fact]
	public async Task GetAllAsync_Void_ReturnsResultResponseDtoCollection()
	{
		// Arrange 
		var results = new List<Result>
		{
			_fixture.Build<Result>().Without(x => x.Appointment).Create()
		};
		var resultsResponse = _resultsMapper.ToResponse(results);

		_unitOfWorkMock.Setup(x => x.ResultRepository.GetAllAsync())
			.ReturnsAsync(results);

		// Act
		var actResult = await _resultService.GetAllAsync();

		// Assert
		_unitOfWorkMock.Verify(X => X.ResultRepository.GetAllAsync(), Times.Once);
	}

	[Fact]
	public async Task GetByIdAsync_ResultId_ReturnsResultResponseDto()
	{
		// Arrange
		var id = Guid.NewGuid();
		var trackChanges = false;
		var result = _fixture.Build<Result>().Without(x => x.Appointment).Create();

		_unitOfWorkMock.Setup(x => x.ResultRepository.GetByIdAsync(id, trackChanges))
			.ReturnsAsync(result);

		// Act
		var actResult = await _resultService.GetByIdAsync(id);

		// Assert
		_unitOfWorkMock.Verify(x => x.ResultRepository.GetByIdAsync(id, trackChanges), Times.Once);
	}

	[Fact]
	public async Task GetByIdAsync_InvalidResultId_ThrowsNotFoundException()
	{
		// Arrange
		var id = new Guid();
		var trackChanges = false;

		_unitOfWorkMock.Setup(x => x.ResultRepository.GetByIdAsync(id, trackChanges))
			.ReturnsAsync(It.IsAny<Result>());

		// Act
		var function = async () => await _resultService.GetByIdAsync(id);

		// Assert
		await Assert.ThrowsAsync<NotFoundException>(function);
	}

	[Fact]
	public async Task UpdateAsync_ResultIdAndValidObjectRequest_ReturnsTask()
	{
		// Arrange
		var resultBeforeUpdate = _fixture.Build<Result>().Without(x => x.Appointment).Create();
		var resultRequest = _fixture.Create<ResultRequestDto>();
		var resultAfterUpdate = _resultsMapper.ToModel(resultRequest);
		resultAfterUpdate.Id = resultBeforeUpdate.Id;

		var id = resultBeforeUpdate.Id;
		var trackChanges = true;

		_unitOfWorkMock.Setup(x => x.ResultRepository.GetByIdAsync(id, trackChanges))
			.ReturnsAsync(resultBeforeUpdate);

		// Act
		await _resultService.UpdateAsync(id, resultRequest);

		// Assert
		_unitOfWorkMock.Verify(x => x.ResultRepository.GetByIdAsync(id, trackChanges), Times.Once);
		Assert.Equivalent(resultBeforeUpdate, resultAfterUpdate);
	}

	[Fact]
	public async Task UpdateAsync_InvalidResultIdAndValidObjectRequest_ThrowsNotFoundException()
	{
		// Arrange
		var resultRequest = _fixture.Create<ResultRequestDto>();

		var id = new Guid();
		var trackChanges = true;

		_unitOfWorkMock.Setup(x => x.ResultRepository.GetByIdAsync(id, trackChanges))
			.ReturnsAsync(It.IsAny<Result>());

		// Act
		var function = async () => await _resultService.UpdateAsync(id, resultRequest);

		// Assert
		await Assert.ThrowsAsync<NotFoundException>(function);
	}
}
