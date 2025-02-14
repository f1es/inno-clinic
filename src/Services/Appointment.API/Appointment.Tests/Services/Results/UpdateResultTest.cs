using Appointment.Application.Mappers.Implementations;
using Appointment.Application.Mappers.Interfaces;
using Appointment.Application.Services.Implementations;
using Appointment.Core.Dto.Request;
using Appointment.Core.Models;
using Appointment.Core.Repositories;
using AutoFixture;
using Moq;
using Shared.Exceptions;

namespace Appointment.Tests.Services.Results;

public class UpdateResultTest
{
	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly IResultsMapper _resultsMapper;
	private readonly Fixture _fixture;

	private readonly ResultService _resultService;

    public UpdateResultTest()
    {
		_unitOfWorkMock = new Mock<IUnitOfWork>();
		_resultsMapper = new ResultsMapper();
		_fixture = new Fixture();

		_resultService = new ResultService(
			_unitOfWorkMock.Object,
			_resultsMapper);
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
