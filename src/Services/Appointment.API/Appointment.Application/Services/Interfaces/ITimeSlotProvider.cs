using Appointment.Application.Models;

namespace Appointment.Application.Services.Interfaces;

public interface ITimeSlotProvider
{
	public List<TimePeriod> SplitFreeTimeByMinutes(int minutes);
	public bool CanAddReservation(TimePeriod timePeriod);
}
