using Appointment.Application.Services.Interfaces;
using Appointment.Application.Models;

namespace Appointment.Application.Services.Implementations;

public class TimeSlotProvider : ITimeSlotProvider
{
	public TimePeriod DayScope { get; private set; }
	public List<TimePeriod> Reservations { get; private set; }
	public List<TimePeriod> FreeTime { get; private set; }

	public TimeSlotProvider(List<TimePeriod> reservations, TimePeriod dayScope)
	{
		Reservations = reservations;
		DayScope = dayScope;
		FreeTime = new List<TimePeriod>();
		UnionPeriods();
		CalculateFreeTime();
	}

	public bool TryAddReservation(TimePeriod reservation)
	{
		if (!CanAddReservation(reservation))
		{
			return false;
		}

		Reservations.Add(reservation);
		UnionPeriods();
		CalculateFreeTime();
		return true;
	}

	public bool CanAddReservation(TimePeriod appointment)
	{
		if (!DayScope.Contains(appointment))
		{
			return false;
		}

		foreach (TimePeriod reservation in Reservations)
		{
			if (reservation.Contains(appointment) || appointment.Contains(reservation))
			{
				return false;
			}
		}

		return true;
	}

	public List<TimePeriod> SplitFreeTimeByMinutes(int minutes)
	{
		var splittedTimes = new List<TimePeriod>();
		var splitTime = new TimeSpan(0, minutes, 0);

		foreach (var timePeriod in FreeTime)
		{
			var periodsCount = (int)(timePeriod / splitTime);

			for (int i = 0; i < periodsCount; i++)
			{
				var beginTime = timePeriod.Begin.Add(splitTime * i);

				var endTime = beginTime.Add(splitTime);

				var appointmentTime = new TimePeriod(timePeriod.Begin.Add(splitTime * i), endTime);
				splittedTimes.Add(appointmentTime);
			}
		}

		return splittedTimes;
	}

	private void OrderByStart() => Reservations = Reservations.OrderBy(x => x.Begin.Ticks).ToList();

	private void UnionPeriods()
	{
		OrderByStart();

		for (int i = 1; i < Reservations.Count; i++)
		{
			if (Reservations[i - 1].End == Reservations[i].Begin)
			{
				Reservations[i] = new TimePeriod(Reservations[i - 1].Begin, Reservations[i].End);
				Reservations.RemoveAt(i - 1);
				i--;
			}
		}
	}

	private void CalculateFreeTime()
	{
		FreeTime.Clear();

		TimeOnly current = DayScope.Begin;

		foreach (var period in Reservations)
		{
			if (current < period.Begin)
			{
				FreeTime.Add(new TimePeriod(current, period.Begin));
			}

			if (current < period.End)
			{
				current = period.End;
			}
		}

		if (current < DayScope.End)
		{
			FreeTime.Add(new TimePeriod(current, DayScope.End));
		}
	}
}
