using Appointment.Application.Mappers.Interfaces;
using Appointment.Application.Services.Implementations;
using Appointment.Core.Repositories;
using AutoFixture;
using Moq;
using Shared.Exceptions;

namespace Appointment.Tests.Services.Appointment;

public class DeleteAppointmentTest
{
	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly Mock<IAppointmentsMapper> _appointmentMapperMock;
    private readonly Fixture _fixture;

	private readonly AppointmentService _appointmentService;

    public DeleteAppointmentTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _appointmentMapperMock = new Mock<IAppointmentsMapper>();

		_fixture = new Fixture();
		_fixture.Register(() => DateOnly.FromDateTime(_fixture.Create<DateTime>()));
		_fixture.Register(() => TimeOnly.FromDateTime(_fixture.Create<DateTime>()));

        _appointmentService = new AppointmentService(
            _unitOfWorkMock.Object,
            _appointmentMapperMock.Object);
    }

    [Fact]
    public async Task DeleteAsync_AppointmentId_ReturnsTask()
    {
        // Arrange
        var id = Guid.NewGuid();
		var appointment = _fixture.Build<Core.Models.Appointment>().Without(x => x.Result).Create();
        var trackChange = false;

        _unitOfWorkMock.Setup(x => x.AppointmentRepository.GetByIdAsync(id, trackChange))
            .ReturnsAsync(appointment);

        _unitOfWorkMock.Setup(x => x.AppointmentRepository.Delete(appointment));

        // Act 
        await _appointmentService.DeleteAsync(id);

        // Assert
        _unitOfWorkMock.Verify(x => x.AppointmentRepository.GetByIdAsync(id, trackChange), Times.Once);
        _unitOfWorkMock.Verify(x => x.AppointmentRepository.Delete(appointment), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_InvalidId_ThrowsNotFoundException()
    {
        // Arrange
        var id = new Guid();
        var trackChanges = false;  

        _unitOfWorkMock.Setup(x => x.AppointmentRepository.GetByIdAsync(id, trackChanges))
            .ReturnsAsync(It.IsAny<Core.Models.Appointment>());

        // Act 
        var function = async () => await _appointmentService.DeleteAsync(id);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(function);
    }
}
