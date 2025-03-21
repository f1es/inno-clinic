using Appointment.Application.Models;
using Appointment.Application.Services.Implementations;

namespace Appointment.Tests.Services;

public class TimeSlotProviderTests
{
    [Fact]
    public void CanAddReservation_ValidTimePeriod_ReturnsTrue()
    {
		// Arrange
		var dayScope = new TimePeriod("01:00:00", "03:00:00");
		var reservations = new List<TimePeriod>()
        {
            new TimePeriod("01:00:00", "01:20:00"),
            new TimePeriod("02:00:00", "03:00:00"),
		};
        var timeSlotProvider = new TimeSlotProvider(reservations, dayScope);
        var reservation = new TimePeriod("01:20:00", "01:40:00");

        // Act 
        var result = timeSlotProvider.CanAddReservation(reservation);

        // Assert
        Assert.True(result);
    }

    public static IEnumerable<object[]> IncorrectTimePeriods()
    {
        yield return new object[] { new TimePeriod("00:00:00", "23:59:00") };
		yield return new object[] { new TimePeriod("00:00:00", "00:00:00") };
		yield return new object[] { new TimePeriod("08:00:00", "10:00:00") };
		yield return new object[] { new TimePeriod("09:00:00", "13:00:00") };
		yield return new object[] { new TimePeriod("10:00:00", "13:00:00") };
		yield return new object[] { new TimePeriod("08:00:00", "09:00:00") };
		yield return new object[] { new TimePeriod("12:00:00", "13:00:00") };
	}

	[Theory]
    [MemberData(nameof(IncorrectTimePeriods))]
    public void CanAddReservation_InvalidTimePeriods_ReturnsFalse(TimePeriod reservation)
    {
        // Arrange
        var dayScope = new TimePeriod("09:00:00", "12:00:00");
		var reservations = new List<TimePeriod>()
		{
			new TimePeriod("09:00:00", "09:20:00"),
			new TimePeriod("10:00:00", "11:00:00"),
		};
		var timeSlotProvider = new TimeSlotProvider(reservations, dayScope);

		// Act
        var result = timeSlotProvider.CanAddReservation(reservation);

		// Assert
        Assert.False(result);
	}

    [Fact]
    public void SplitFreeTimeByMinutes_ValidIntValue_ReturnsListOfTimePeriods()
    {
		// Arrange
		var dayScope = new TimePeriod("09:00:00", "12:00:00");
		var reservations = new List<TimePeriod>()
		{
			new TimePeriod("10:00:00", "10:30:00"),
		};
		var timeSlotProvider = new TimeSlotProvider(reservations, dayScope);
		var minutes = 30;
		var expectedPeriods = new List<TimePeriod>()
		{
			new TimePeriod("09:00:00", "09:30:00"),
			new TimePeriod("09:30:00", "10:00:00"),
			new TimePeriod("10:30:00", "11:00:00"),
			new TimePeriod("11:00:00", "11:30:00"),
			new TimePeriod("11:30:00", "12:00:00"),
		};

		// Act
		var result = timeSlotProvider.SplitFreeTimeByMinutes(minutes);

		// Assert
		Assert.Equivalent(5, result.Count);
		Assert.Equivalent(expectedPeriods, result);
	}

	[Fact]
	public void SplitFreeTimeByMinutes_InvalidIntValue_ReturnsEmptyListOfTimePeriods()
	{
		// Arrange
		var dayScope = new TimePeriod("09:00:00", "12:00:00");
		var reservations = new List<TimePeriod>()
		{
			new TimePeriod("10:00:00", "10:30:00"),
		};
		var timeSlotProvider = new TimeSlotProvider(reservations, dayScope);
		var minutes = 10000000;

		// Act
		var result = timeSlotProvider.SplitFreeTimeByMinutes(minutes);

		// Assert
		Assert.Equivalent(0, result.Count);
	}

	[Fact]
	public void SplitFreeTimeByMinutes_InvalidIntValue_Returns()
	{
		// Arrange
		var dayScope = new TimePeriod("09:00:00", "12:00:00");
		var timeSlotProvider = new TimeSlotProvider([], dayScope);
		var minutes = 0;

		// Act & Assert
		Assert.Throws<DivideByZeroException>(() => timeSlotProvider.SplitFreeTimeByMinutes(minutes));
	}
}
