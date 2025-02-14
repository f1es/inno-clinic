using Appointment.Application.Mappers.Implementations;
using Appointment.Application.Mappers.Interfaces;
using Appointment.Application.Services.Implementations;
using Appointment.Core.Repositories;
using AutoFixture;
using Moq;
using Shared.Exceptions;

namespace Appointment.Tests.Services.Appointment;

public class GetAppointmentByIdTest
{
	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly IAppointmentsMapper _mapper;
    private readonly Fixture _fixture;

	private readonly AppointmentService _appointmentService;

    public GetAppointmentByIdTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapper = new AppointmentsMapper();

        _fixture = new Fixture();
		_fixture.Register(() => DateOnly.FromDateTime(_fixture.Create<DateTime>()));
		_fixture.Register(() => TimeOnly.FromDateTime(_fixture.Create<DateTime>()));

		_appointmentService = new AppointmentService(
            _unitOfWorkMock.Object, 
            _mapper);
    }

    [Fact]
    public async Task GetByIdAsync_AppointmentId_ReturnsAppointmentResponseDto()
    {
        // Arrange
        var id = Guid.NewGuid();
        var appointment = _fixture.Build<Core.Models.Appointment>().Without(x => x.Result).Create();
        appointment.Id = id;
        var trackChanges = false;

        var appointmentResponse = _mapper.ToResponse(appointment);

        _unitOfWorkMock.Setup(x => x.AppointmentRepository.GetByIdAsync(id, trackChanges))
            .ReturnsAsync(appointment);

        // Act
        var result = await _appointmentService.GetByIdAsync(id);

        // Assert
        _unitOfWorkMock.Verify(x => x.AppointmentRepository.GetByIdAsync(id, trackChanges), Times.Once);

        Assert.Equivalent(appointmentResponse, result);
    }

    [Fact]
    public async Task GetByIdAsync_InvalidAppointmentId_ThrowsNotFoundException()
    {
        // Arrange
        var id = new Guid();
        var trackChanges = false;

        _unitOfWorkMock.Setup(x => x.AppointmentRepository.GetByIdAsync(id, trackChanges))
            .ReturnsAsync(It.IsAny<Core.Models.Appointment>());
        // Act
        var function = async () => await _appointmentService.GetByIdAsync(id);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(function);
    }
}
