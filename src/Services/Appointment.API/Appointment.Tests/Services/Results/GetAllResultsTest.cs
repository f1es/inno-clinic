using Appointment.Application.Mappers.Implementations;
using Appointment.Application.Mappers.Interfaces;
using Appointment.Application.Services.Implementations;
using Appointment.Core.Models;
using Appointment.Core.Repositories;
using AutoFixture;
using Moq;

namespace Appointment.Tests.Services.Results;

public class GetAllResultsTest
{
	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly IResultsMapper _resultsMapper;
	private readonly Fixture _fixture;

	private readonly ResultService _resultService;

    public GetAllResultsTest()
    {
		_unitOfWorkMock = new Mock<IUnitOfWork>();
		_resultsMapper = new ResultsMapper();
		_fixture = new Fixture();

		_resultService = new ResultService(
			_unitOfWorkMock.Object,
			_resultsMapper);
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
}
