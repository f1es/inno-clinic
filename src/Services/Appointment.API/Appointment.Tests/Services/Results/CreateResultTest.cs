using Appointment.Application.Mappers.Implementations;
using Appointment.Application.Mappers.Interfaces;
using Appointment.Application.Services.Implementations;
using Appointment.Core.Dto.Request;
using Appointment.Core.Models;
using Appointment.Core.Repositories;
using AutoFixture;
using Moq;

namespace Appointment.Tests.Services.Results;

public class CreateResultTest
{
	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly IResultsMapper _resultsMapper;
	private readonly Fixture _fixture;

	private readonly ResultService _resultService;

    public CreateResultTest()
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
}
