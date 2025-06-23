using Appointment.Application.Mappers.Implementations;
using Appointment.Application.Mappers.Interfaces;
using Appointment.Application.Services.Implementations;
using Appointment.Application.Services.Interfaces;
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
	private readonly Mock<IPdfService> _pdfServiceMock;
	private readonly IResultsMapper _resultsMapper;
	private readonly Fixture _fixture;

	private readonly ResultService _resultService;

    public ResultServiceTests()
    {
		_unitOfWorkMock = new Mock<IUnitOfWork>();
		_pdfServiceMock = new Mock<IPdfService>();
		_resultsMapper = new ResultsMapper();
		_fixture = new Fixture();

		_resultService = new ResultService(
			_unitOfWorkMock.Object,
			_resultsMapper,
			_pdfServiceMock.Object,
			null);
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

		_unitOfWorkMock.Setup(x => x.ResultRepository.GetAllAsync(CancellationToken.None))
			.ReturnsAsync(results);

		// Act
		var actResult = await _resultService.GetAllAsync(CancellationToken.None);

		// Assert
		_unitOfWorkMock.Verify(x => x.ResultRepository.GetAllAsync(CancellationToken.None), Times.Once);
		Assert.Equivalent(resultsResponse, actResult);
	}

	[Fact]
	public async Task GetByIdAsync_ResultId_ReturnsResultResponseDto()
	{
		// Arrange
		var id = Guid.NewGuid();
		var trackChanges = false;
		var result = _fixture.Build<Result>().Without(x => x.Appointment).Create();

		_unitOfWorkMock.Setup(x => x.ResultRepository.GetByIdAsync(id, CancellationToken.None, trackChanges))
			.ReturnsAsync(result);

		// Act
		var actResult = await _resultService.GetByIdAsync(id, CancellationToken.None);

		// Assert
		_unitOfWorkMock.Verify(x => x.ResultRepository.GetByIdAsync(id, CancellationToken.None, trackChanges), Times.Once);
		Assert.Equivalent(_resultsMapper.ToResponse(result), result);
	}

	[Fact]
	public async Task GetByIdAsync_InvalidResultId_ThrowsNotFoundException()
	{
		// Arrange
		var id = new Guid();
		var trackChanges = false;

		_unitOfWorkMock.Setup(x => x.ResultRepository.GetByIdAsync(id, CancellationToken.None, trackChanges))
			.ReturnsAsync(It.IsAny<Result>());

		// Act
		var function = async () => await _resultService.GetByIdAsync(id, CancellationToken.None);

		// Assert
		await Assert.ThrowsAsync<NotFoundException>(function);
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
		var actResult = await _resultService.CreateAsync(resultRequest, CancellationToken.None);

		// Assert
		_unitOfWorkMock.Verify(x => x.ResultRepository.Create(It.IsAny<Result>()), Times.Once);

		Assert.Equivalent(resultResponse, actResult);
	}

	[Fact]
	public async Task UpdateAsync_ResultIdAndValidObjectRequest_ReturnsTask()
	{
		// Arrange
		var resultInDb = _fixture.Build<Result>().Without(x => x.Appointment).Create();
		var resultRequest = _fixture.Create<ResultRequestDto>();
		var resultAfterUpdate = _resultsMapper.ToModel(resultRequest);
		resultAfterUpdate.Id = resultInDb.Id;

		var id = resultInDb.Id;
		var trackChanges = true;

		_unitOfWorkMock.Setup(x => x.ResultRepository.GetByIdAsync(id, CancellationToken.None, trackChanges))
			.ReturnsAsync(resultInDb);

		// Act
		await _resultService.UpdateAsync(id, resultRequest, CancellationToken.None);

		// Assert
		_unitOfWorkMock.Verify(x => x.ResultRepository.GetByIdAsync(id, CancellationToken.None, trackChanges), Times.Once);
		Assert.Equivalent(resultInDb, resultAfterUpdate);
	}

	[Fact]
	public async Task UpdateAsync_InvalidResultIdAndValidObjectRequest_ThrowsNotFoundException()
	{
		// Arrange
		var resultRequest = _fixture.Create<ResultRequestDto>();

		var id = new Guid();
		var trackChanges = true;

		_unitOfWorkMock.Setup(x => x.ResultRepository.GetByIdAsync(id, CancellationToken.None, trackChanges))
			.ReturnsAsync(It.IsAny<Result>());

		// Act
		var function = async () => await _resultService.UpdateAsync(id, resultRequest, CancellationToken.None);

		// Assert
		await Assert.ThrowsAsync<NotFoundException>(function);
	}

	[Fact]
	public async Task DeleteAsync_ResultId_ReturnsTask()
	{
		// Arrange
		var result = _fixture.Build<Result>().Without(x => x.Appointment).Create();
		var id = Guid.NewGuid();
		var trackChanges = false;

		_unitOfWorkMock.Setup(x => x.ResultRepository.GetByIdAsync(id, CancellationToken.None, trackChanges)).
			ReturnsAsync(result);
		_unitOfWorkMock.Setup(x => x.ResultRepository.Delete(result));

		// Act
		await _resultService.DeleteAsync(id, CancellationToken.None);

		// Assert
		_unitOfWorkMock.Verify(x => x.ResultRepository.GetByIdAsync(id, CancellationToken.None, trackChanges), Times.Once);
		_unitOfWorkMock.Verify(x => x.ResultRepository.Delete(result), Times.Once);
	}

	[Fact]
	public async Task DeleteAsync_InvalidResultId_ThrowsNotFoundException()
	{
		// Arrange
		var id = new Guid();
		var trackChanges = false;
		var result = _fixture.Build<Result>().Without(x => x.Appointment).Create();

		_unitOfWorkMock.Setup(x => x.ResultRepository.GetByIdAsync(id, CancellationToken.None, trackChanges)).
			ReturnsAsync(It.IsAny<Result>());
		_unitOfWorkMock.Setup(x => x.ResultRepository.Delete(result));

		// Act
		var function = async () => await _resultService.DeleteAsync(id, CancellationToken.None);

		// Assert
		await Assert.ThrowsAsync<NotFoundException>(function);
	}
}
