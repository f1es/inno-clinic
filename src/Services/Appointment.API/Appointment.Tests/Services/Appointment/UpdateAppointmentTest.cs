using Appointment.Application.Mappers.Implementations;
using Appointment.Application.Mappers.Interfaces;
using Appointment.Application.Services.Implementations;
using Appointment.Core.Dto.Request;
using Appointment.Core.Repositories;
using AutoFixture;
using Moq;
using Shared.Exceptions;

namespace Appointment.Tests.Services.Appointment;

public class UpdateAppointmentTest
{
	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly IAppointmentsMapper _appointmentsMapper;
	private readonly Fixture _fixture;

	private readonly AppointmentService _appointmentService;

    public UpdateAppointmentTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
		_appointmentsMapper = new AppointmentsMapper();
		_fixture = new Fixture();
		_fixture.Register(() => DateOnly.FromDateTime(_fixture.Create<DateTime>()));
		_fixture.Register(() => TimeOnly.FromDateTime(_fixture.Create<DateTime>()));

		_appointmentService = new AppointmentService(
			_unitOfWorkMock.Object, 
			_appointmentsMapper);
	}

	[Fact]
	public async Task UpdateAsync_AppointmentIdAndValidObjectRequest_ReturnsVoid()
	{
		// Arrange
		var appointmentRequest = _fixture.Create<AppointmentRequestDto>();
		var appointment = _fixture.Build< Core.Models.Appointment>().Without(x => x.Result).Create();
		var appointmentAfterUpdate = _appointmentsMapper.ToModel(appointmentRequest);
		appointmentAfterUpdate.Id = appointment.Id;
		var trackChanges = true;

		var id = Guid.NewGuid();
		_unitOfWorkMock.Setup(x => x.AppointmentRepository.GetByIdAsync(id, trackChanges))
			.ReturnsAsync(appointment);

		// Act 
		await _appointmentService.UpdateAsync(id, appointmentRequest);

		// Assert
		_unitOfWorkMock.Verify(x => x.AppointmentRepository.GetByIdAsync(id, trackChanges), Times.Once);

		Assert.Equivalent(appointment, appointmentAfterUpdate);
	}

	[Fact]
	public async Task UpdateAsync_InvalidAppointmentIdAndValidObjectRequest_ThrowsNotFoundException()
	{
		// Arrange
		var appointmentRequest = _fixture.Create<AppointmentRequestDto>();
		var id = new Guid();
		var trackChanges = false;

		_unitOfWorkMock.Setup(x => x.AppointmentRepository.GetByIdAsync(id, trackChanges))
			.ReturnsAsync(It.IsAny<Core.Models.Appointment>());

		// Act
		var function = async () => await _appointmentService.UpdateAsync(id, appointmentRequest);

		// Assert
		await Assert.ThrowsAsync<NotFoundException>(function);
	}
}
