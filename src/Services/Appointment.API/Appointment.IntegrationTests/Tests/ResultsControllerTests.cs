using Appointment.Core.Dto.Request;
using Appointment.Core.Dto.Response;
using Appointment.Core.Models;
using Appointment.IntegrationTests.Utility;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using System.Net;

namespace Appointment.IntegrationTests.Tests;

public class ResultsControllerTests : ResultControllerTestBase
{
	private readonly Fixture _fixture;

    public ResultsControllerTests()
    {
        _fixture = new Fixture();

		_fixture.Register(() => DateOnly.FromDateTime(_fixture.Create<DateTime>()));
		_fixture.Register(() => new TimeOnly(1, 1, 1));
	}

	[Fact]
	public async Task GetAll_StatusCodeOk()
	{
		// Arrange 
		var cancellationToken = new CancellationToken();
		var result = await AddResultToDbAsync(cancellationToken);
		var expectedResult = _resultsMapper.ToResponse(result);

		// Act
		var response = await _resultController.GetAll(cancellationToken);

		// Assert
		var objectResponse = response as ObjectResult;
		var collectionResult = objectResponse.Value as IEnumerable<ResultResponseDto>;
		Assert.Equal((int)HttpStatusCode.OK, objectResponse.StatusCode);
		Assert.Equivalent(expectedResult, collectionResult.First());
	}

	[Fact]
	public async Task Get_StatusCodeOk()
	{
		// Arrange
		var cancellationToken = new CancellationToken();
		var result = await AddResultToDbAsync(cancellationToken);
		var expectedResult = _resultsMapper.ToResponse(result);
		var id = result.Id;

		// Act
		var response = await _resultController.Get(id, cancellationToken);

		// Assert
		var objectResponse = response as ObjectResult;
		Assert.Equal((int)HttpStatusCode.OK, objectResponse.StatusCode);
		Assert.Equivalent(expectedResult, objectResponse.Value);
	}

	[Fact]
	public async Task Get_ThrowsNotFoundException()
	{
		// Arrange
		var cancellationToken = new CancellationToken();
		var id = new Guid();

		// Act
		var function = async () => await _resultController.Get(id, cancellationToken);

		// Assert
		await Assert.ThrowsAsync<NotFoundException>(function);
	}

	[Fact]
	public async Task Create_ValidRequestObject_StatusCodeCreated()
	{
		// Arrange
		var cancellationToken = new CancellationToken();

		var appointment = _fixture.Build<Core.Models.Appointment>().Without(x => x.Result).Create();
		_unitOfWork.AppointmentRepository.Create(appointment);
		await _unitOfWork.SaveAsync(cancellationToken);

		var resultRequestDto = _fixture.Build<ResultRequestDto>()
			.With(x => x.AppointmentId, appointment.Id)
			.Create();

		// Act
		var response = await _resultController.Create(resultRequestDto, cancellationToken);

		// Assert
		var responseObject = response as ObjectResult;
		Assert.Equal((int)HttpStatusCode.Created, responseObject.StatusCode);
	}

	[Fact]
	public async Task Create_ValidRequestObjectWithInvalidForeignKey_ThrowsDbUpdateException()
	{
		// Arrange
		var cancellationToken = new CancellationToken();
		var resultRequestDto = _fixture.Create<ResultRequestDto>();

		// Act
		var function = async () => await _resultController.Create(resultRequestDto, cancellationToken);

		// Assert
		await Assert.ThrowsAsync<DbUpdateException>(function);
	}

	[Fact]
	public async Task Delete_ValidId_StatusCodeNoContent()
	{
		// Arrange
		var cancellationToken = new CancellationToken();
		var resultInDb = await AddResultToDbAsync(cancellationToken);
		var id = resultInDb.Id;

		// Act
		var response = await _resultController.Delete(id, cancellationToken);

		// Assert
		var responseObject = response as NoContentResult;
		Assert.Equal((int)HttpStatusCode.NoContent, responseObject.StatusCode);
	}

	[Fact]
	public async Task Delete_InvalidId_ThrowsNotFoundException()
	{
		// Arrange
		var cancellationToken = new CancellationToken();
		var id = new Guid();

		// Act
		var function = async () => await _resultController.Delete(id, cancellationToken);

		// Assert
		await Assert.ThrowsAsync<NotFoundException>(function);
	}

	[Fact]
	public async Task Update_ValidIdAndValidRequestObject_StatusCodeNoContent()
	{
		// Arrange
		var cancellationToken = new CancellationToken();
		var resultInDb = await AddResultToDbAsync(cancellationToken);
		var id = resultInDb.Id;
		var appointmentId = resultInDb.AppointmentId;
		var resultRequestDto = _fixture.Build<ResultRequestDto>()
			.With(x => x.AppointmentId, appointmentId)
			.Create();

		// Act
		var response = await _resultController.Update(id, resultRequestDto, cancellationToken);

		// Assert
		var responseObject = response as NoContentResult;
		Assert.Equal((int)HttpStatusCode.NoContent, responseObject.StatusCode);
	}

	[Fact]
	public async Task Update_InvalidIdAndValidRequestObject_ThrowsNotFoundException()
	{
		// Arrange
		var cancellationToken = new CancellationToken();
		var resultInDb = await AddResultToDbAsync(cancellationToken);
		var id = new Guid();
		var appointmentId = resultInDb.AppointmentId;
		var resultRequestDto = _fixture.Build<ResultRequestDto>()
			.With(x => x.AppointmentId, appointmentId)
			.Create();

		// Act
		var function = async () => await _resultController.Update(id, resultRequestDto, cancellationToken);

		// Assert
		await Assert.ThrowsAsync<NotFoundException>(function);
	}

	[Fact]
	public async Task Update_ValidIdAndValidRequestObjectWithInvalidForeignKey_ThrowsDbUpdateException()
	{
		// Arrange
		var cancellationToken = new CancellationToken();
		var resultInDb = await AddResultToDbAsync(cancellationToken);
		var id = resultInDb.Id;
		var appointmentId = new Guid();
		var resultRequestDto = _fixture.Build<ResultRequestDto>()
			.With(x => x.AppointmentId, appointmentId)
			.Create();

		// Act
		var function = async () => await _resultController.Update(id, resultRequestDto, cancellationToken);

		// Assert
		await Assert.ThrowsAsync<DbUpdateException>(function);
	}

	[Fact]
	public async Task Download_StatusCodeOk()
	{
		// Arrange
		var cancellationToken = new CancellationToken();
		var result = await AddResultToDbAsync(cancellationToken);
		var expectedResult = _resultsMapper.ToResponse(result);
		var id = result.Id;

		// Act
		var response = await _resultController.Download(id, cancellationToken);

		// Assert
		Assert.Equal(typeof(FileContentResult), response.GetType());
		var objectResponse = response as FileContentResult;
		Assert.Equal("text/plain", objectResponse.ContentType);
	}

	[Fact]
	public async Task Download_ThrowsNotFoundException()
	{
		// Arrange
		var cancellationToken = new CancellationToken();
		var id = new Guid();

		// Act
		var function = async () => await _resultController.Download(id, cancellationToken);

		// Assert
		await Assert.ThrowsAsync<NotFoundException>(function);
	}

	private async Task<Result> AddResultToDbAsync(CancellationToken cancellationToken)
	{
		var appointment = _fixture.Build<Core.Models.Appointment>().Without(x => x.Result).Create();
		var result = _fixture.Build<Result>().Without(x => x.Appointment).Create();
		result.AppointmentId = appointment.Id;

		_unitOfWork.AppointmentRepository.Create(appointment);
		_unitOfWork.ResultRepository.Create(result);
		await _unitOfWork.SaveAsync(cancellationToken);
		_context.Entry(result).State = EntityState.Detached;
		return result;
	}
}
