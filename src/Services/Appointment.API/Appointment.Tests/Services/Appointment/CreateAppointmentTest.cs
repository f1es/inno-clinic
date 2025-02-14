using Appointment.Application.Mappers.Implementations;
using Appointment.Application.Mappers.Interfaces;
using Appointment.Application.Services.Implementations;
using Appointment.Core.Dto.Request;
using Appointment.Core.Repositories;
using AutoFixture;
using Moq;

namespace Appointment.Tests.Services.Appointment;


public class CreateAppointmentTest
{
	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly IAppointmentsMapper _appointmentsMapper;
	private readonly Fixture _fixture;

	private readonly AppointmentService _appointmentService;

    public CreateAppointmentTest()
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
	public async Task CreateAsync_ValidObjectRequest_ReturnsAppointmentResponseDto()
	{
		// Arrange
		_unitOfWorkMock.Setup(x => x.AppointmentRepository.Create(It.IsAny<Core.Models.Appointment>()));
		_unitOfWorkMock.Setup(x => x.SaveAsync());

		var appointmentRequest = _fixture.Create<AppointmentRequestDto>();
		var model = _appointmentsMapper.ToModel(appointmentRequest);
		var response = _appointmentsMapper.ToResponse(model);

		// Act
		var result = await _appointmentService.CreateAsync(appointmentRequest);

		// Assert
		_unitOfWorkMock.Verify(x => 
		x.AppointmentRepository.Create(It.IsAny<Core.Models.Appointment>()), Times.Once);

		Assert.NotNull(result);
		Assert.Equivalent(result, response);
	}
}
