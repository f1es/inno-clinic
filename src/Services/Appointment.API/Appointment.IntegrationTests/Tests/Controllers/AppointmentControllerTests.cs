using Appointment.Core.Dto.Request;
using Appointment.Core.Dto.Response;
using Appointment.IntegrationTests.Utility;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Shared.Exceptions;
using System.Net;

namespace Appointment.IntegrationTests.Tests.Controllers;

public class AppointmentControllerTests : AppointmentControllerTestsBase
{
    private readonly Fixture _fixture;

    public AppointmentControllerTests()
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
        var appointment = await AddAppointmentToDbAsync(cancellationToken);
        var appointmentResponseDto = _appointmentsMapper.ToResponse(appointment);

        // Act
        var response = await _appointmentController.GetAll(cancellationToken);

        // Assert
        var objectResult = response as ObjectResult;
        var collectionResult = objectResult.Value as IEnumerable<AppointmentResponseDto>;
        Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
        Assert.Equivalent(collectionResult.First(), appointmentResponseDto);
    }

    [Fact]
    public async Task Get_StatusCodeOk()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var appointment = await AddAppointmentToDbAsync(cancellationToken);
        var appointmentResponseDto = _appointmentsMapper.ToResponse(appointment);
        var id = appointment.Id;

        // Act
        var response = await _appointmentController.Get(id, cancellationToken);

        // Assert
        var responseObject = response as ObjectResult;
        Assert.Equal((int)HttpStatusCode.OK, responseObject.StatusCode);
        Assert.Equivalent(appointmentResponseDto, responseObject.Value);
    }

    [Fact]
    public async Task Get_ThrowsNotFoundException()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var id = new Guid();

        // Act
        var function = async () => await _appointmentController.Get(id, cancellationToken);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(function);
    }

    [Fact]
    public async Task Create_ValidRequestObject_StatusCodeCreated()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var appointmentRequestDto = _fixture.Build<AppointmentRequestDto>().Create();

        // Act
        var response = await _appointmentController.Create(appointmentRequestDto, cancellationToken);

        // Assert
        var responseObject = response as ObjectResult;
        Assert.Equal((int)HttpStatusCode.Created, responseObject.StatusCode);
    }

    [Fact]
    public async Task Update_ValidRequestObjectAndValidId_StatusCodeNoContent()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var appointmentRequestDto = _fixture.Build<AppointmentRequestDto>().Create();
        var appointmentInDb = await AddAppointmentToDbAsync(cancellationToken);
        var id = appointmentInDb.Id;

        // Act
        var response = await _appointmentController.Update(id, appointmentRequestDto, cancellationToken);

        // Assert
        var responseObject = response as NoContentResult;
        Assert.Equal((int)HttpStatusCode.NoContent, responseObject.StatusCode);
    }

    [Fact]
    public async Task Update_ValidRequestObjectAndInvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var appointmentRequestDto = _fixture.Build<AppointmentRequestDto>().Create();
        var id = new Guid();

        // Act
        var function = async () => await _appointmentController.Update(id, appointmentRequestDto, cancellationToken);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(function);
    }

    [Fact]
    public async Task Delete_ValidId_StatusCodeNoContent()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var appointmentInDb = await AddAppointmentToDbAsync(cancellationToken);
        var id = appointmentInDb.Id;

        // Act
        var response = await _appointmentController.Delete(id, cancellationToken);

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
        var function = async () => await _appointmentController.Delete(id, cancellationToken);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(function);
    }

    private async Task<Core.Models.Appointment> AddAppointmentToDbAsync(CancellationToken cancellationToken)
    {
        var appointment = _fixture.Build<Core.Models.Appointment>().Without(x => x.Result).Create();
        _unitOfWork.AppointmentRepository.Create(appointment);

        await _unitOfWork.SaveAsync(cancellationToken);
        _context.Entry(appointment).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        return appointment;
    }
}
