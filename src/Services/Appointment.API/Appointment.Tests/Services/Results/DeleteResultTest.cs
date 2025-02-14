using Appointment.Application.Mappers.Implementations;
using Appointment.Application.Mappers.Interfaces;
using Appointment.Application.Services.Implementations;
using Appointment.Core.Models;
using Appointment.Core.Repositories;
using AutoFixture;
using Moq;
using Shared.Exceptions;

namespace Appointment.Tests.Services.Results;

public class DeleteResultTest
{
	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly IResultsMapper _resultsMapper;
	private readonly Fixture _fixture;

	private readonly ResultService _resultService;

    public DeleteResultTest()
    {
		_unitOfWorkMock = new Mock<IUnitOfWork>();
		_resultsMapper = new ResultsMapper();
		_fixture = new Fixture();

		_resultService = new ResultService(
			_unitOfWorkMock.Object,
			_resultsMapper);
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
}
