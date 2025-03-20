using Appointment.Application.Services.Interfaces;
using Appointment.Application.Models;
using Appointment.Core.Dto.Request;
using Appointment.Core.Repositories;
using Shared.Exceptions;

namespace Appointment.Application.Services.Implementations;
public class TimeSlotService : ITimeSlotService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly TimePeriod _workingDayPeriod = new TimePeriod("09:00:00", "18:00:00");

	public TimeSlotService(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<bool> CheckIfSlotAvailableAsync(Core.Models.Appointment appointment, CancellationToken cancellationToken)
	{
		if (appointment.BeginTime > appointment.EndTime)
		{
			throw new BadRequestException("Incorrect time values, begin cannot be more than end");
		}

		var timeSlotProvider = await GetTimeSlotProviderForDayAsync(appointment.Date, cancellationToken);

		var appointmentTime = new TimePeriod(appointment.BeginTime, appointment.EndTime);

		return timeSlotProvider.CanAddReservation(appointmentTime);
	}

	public async Task<IEnumerable<TimePeriod>> GetAvailablePeriodsForDateAsync(AvailableTimesRequestDto availableTimesRequestDto, CancellationToken cancellationToken)
	{
		var timeSlotProvider = await GetTimeSlotProviderForDayAsync(availableTimesRequestDto.Date, cancellationToken);

		return timeSlotProvider.SplitFreeTimeByMinutes(availableTimesRequestDto.Minutes);
	}

	private async Task<ITimeSlotProvider> GetTimeSlotProviderForDayAsync(DateOnly date, CancellationToken cancellationToken)
	{
		var appointmentsOfDate = await _unitOfWork.AppointmentRepository.GetAllByDayAsync(date, cancellationToken);
		var reservedTimes = appointmentsOfDate.Select(x => new TimePeriod(x.BeginTime, x.EndTime)).ToList();

		return new TimeSlotProvider(reservedTimes, _workingDayPeriod);
	}
}
