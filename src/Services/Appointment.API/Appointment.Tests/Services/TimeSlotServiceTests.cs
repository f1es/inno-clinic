using Appointment.Application.Models;
using Appointment.Application.Services.Implementations;
using Appointment.Core.Dto.Request;
using Appointment.Core.Models;
using Appointment.Core.Repositories;
using AutoFixture;
using Moq;

namespace Appointment.Tests.Services;

public class TimeSlotServiceTests
{
    private Fixture _fixture;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private TimeSlotService _timeSlotService;

    public TimeSlotServiceTests()
    {
        _fixture = new Fixture();
		_fixture.Register(() => DateOnly.FromDateTime(_fixture.Create<DateTime>()));
		_fixture.Register(() => TimeOnly.FromDateTime(_fixture.Create<DateTime>()));
        _fixture.Register(() => new Result());

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _timeSlotService = new TimeSlotService(_unitOfWorkMock.Object);
	}

    [Fact]
    public async Task CheckSlotIfAvailableAsync_AvailableAppointment_ReturnsTrue()
    {
		// Arrange
		var appointments = new List<Core.Models.Appointment>()
        {
		    CreateAppointmentWithPeriod(new TimePeriod("09:00:00", "09:30:00")),
			CreateAppointmentWithPeriod(new TimePeriod("10:00:00", "10:30:00"))
		};

		_unitOfWorkMock.Setup(x => x.AppointmentRepository.GetAllByDayAsync(It.IsAny<DateOnly>(), CancellationToken.None)).
			ReturnsAsync(appointments);

		var appointment = CreateAppointmentWithPeriod(new TimePeriod("14:00:00", "15:00:00"));

		// Act
		var result = await _timeSlotService.CheckIfSlotAvailableAsync(appointment, CancellationToken.None);

		// Assert
        Assert.True(result);
	}

	[Fact]
	public async Task CheckSlotIfAvailableAsync_UnavailableAppointment_ReturnsFalse()
	{
		// Arrange
		var appointments = new List<Core.Models.Appointment>()
		{
			CreateAppointmentWithPeriod(new TimePeriod("09:00:00", "09:30:00")),
			CreateAppointmentWithPeriod(new TimePeriod("10:00:00", "10:30:00"))
		};

		_unitOfWorkMock.Setup(x => x.AppointmentRepository.GetAllByDayAsync(It.IsAny<DateOnly>(), CancellationToken.None)).
			ReturnsAsync(appointments);

		var appointment = CreateAppointmentWithPeriod(new TimePeriod("09:00:00", "11:00:00"));

		// Act
		var result = await _timeSlotService.CheckIfSlotAvailableAsync(appointment, CancellationToken.None);

		// Assert
		Assert.False(result);
	}

	[Fact] 
	public async Task GetAvailablePeriodsForDateAsync_AvailableTimeRequestDto_ReturnsListOfTimePeriods()
	{
		// Arrange 
		var appointments = new List<Core.Models.Appointment>()
		{
			CreateAppointmentWithPeriod(new TimePeriod("09:00:00", "14:00:00")),
			CreateAppointmentWithPeriod(new TimePeriod("16:00:00", "18:00:00"))
		};

		_unitOfWorkMock.Setup(x => x.AppointmentRepository.GetAllByDayAsync(It.IsAny<DateOnly>(), CancellationToken.None)).
			ReturnsAsync(appointments);

		var availableTimeRequest = new AvailableTimesRequestDto(It.IsAny<DateOnly>(), 60);

		// Act
		var result = await _timeSlotService.GetAvailablePeriodsForDateAsync(availableTimeRequest, CancellationToken.None);

		// Assert
		var expectedPeriods = new List<TimePeriod>()
		{
			new TimePeriod("14:00:00", "15:00:00"),
			new TimePeriod("15:00:00", "16:00:00"),
		};
		Assert.Equivalent(expectedPeriods, result);
		Assert.Equivalent(2, result.Count());
	}

	private Core.Models.Appointment CreateAppointmentWithPeriod(TimePeriod period) =>
        _fixture.Build<Core.Models.Appointment>()
            .With(x => x.BeginTime, period.Begin)
            .With(x => x.EndTime, period.End)
            .Create();
}
