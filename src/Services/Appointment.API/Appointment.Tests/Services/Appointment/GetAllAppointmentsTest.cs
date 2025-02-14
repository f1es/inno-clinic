using Appointment.Application.Mappers.Implementations;
using Appointment.Application.Mappers.Interfaces;
using Appointment.Application.Services.Implementations;
using Appointment.Core.Repositories;
using AutoFixture;
using Moq;

namespace Appointment.Tests.Services.Appointment;

public class GetAllAppointmentsTest
{
	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly IAppointmentsMapper _appointmentsMapper;
	private readonly Fixture _fixture;

	private readonly AppointmentService _appointmentService;

    public GetAllAppointmentsTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
		_appointmentsMapper = new AppointmentsMapper();

		_appointmentService = new AppointmentService(
			_unitOfWorkMock.Object,
			_appointmentsMapper);
		_fixture = new Fixture();
		_fixture.Register(() => DateOnly.FromDateTime(_fixture.Create<DateTime>()));
		_fixture.Register(() => TimeOnly.FromDateTime(_fixture.Create<DateTime>()));
	}

	[Fact]
	public async Task GetAllAsync_Void_ReturnsAppointmentResponseDtoCollection()
	{
		// Arrange 
		var appointmentsCollection = new List<Core.Models.Appointment>
		{
			_fixture.Build<Core.Models.Appointment>().Without(x => x.Result).Create(),
		};
		var responseDtoCollection = _appointmentsMapper.ToResponse(appointmentsCollection);

		_unitOfWorkMock.Setup(x => x.AppointmentRepository.GetAllAsync())
			.ReturnsAsync(appointmentsCollection);

		// Act 
		var result = await _appointmentService.GetAllAsync();

		// Assert

		_unitOfWorkMock.Verify(x => x.AppointmentRepository.GetAllAsync(), Times.Once);

		Assert.NotNull(result);
		Assert.Equivalent(responseDtoCollection, result);
	}
}
