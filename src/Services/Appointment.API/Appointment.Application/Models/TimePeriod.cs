namespace Appointment.Application.Models;

public class TimePeriod
{
	public TimeOnly Begin { get; private set; }
	public TimeOnly End { get; private set; }

	public TimePeriod(TimeOnly begin, TimeOnly end)
	{
		Begin = begin;
		End = end;

		if (Begin > End)
		{
			throw new ArgumentException("Incorrect time values, begin time cannot be more than end time");
		}
	}

	public TimePeriod(string begin, string end)
	{
		Begin = TimeOnly.Parse(begin);
		End = TimeOnly.Parse(end);

		if (Begin > End)
		{
			throw new ArgumentException("Incorrect time values, begin time cannot be more than end time");
		}
	}

	public static double operator /(TimePeriod timePeriod, TimeSpan time) =>
		(timePeriod.End.Ticks - timePeriod.Begin.Ticks) / time.Ticks;

	public bool Contains(TimePeriod period)
	{
		if (period.Begin == Begin && period.End == End)
		{
			return true;
		}

		var isBeginInRange = period.Begin < End && period.Begin > Begin;
		var isEndInRange = period.End < End && period.End > Begin;

		return isBeginInRange || isEndInRange;
	}

	public override string ToString() => $"{Begin}-{End}";
}