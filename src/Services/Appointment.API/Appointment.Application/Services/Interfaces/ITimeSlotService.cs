using Appointment.Application.Models;
using Appointment.Core.Dto.Request;

namespace Appointment.Application.Services.Interfaces;

public interface ITimeSlotService
{
	public Task<bool> CheckIfSlotAvailableAsync(Core.Models.Appointment appointment, CancellationToken cancellationToken);
	public Task<IEnumerable<TimePeriod>> GetAvailablePeriodsForDateAsync(AvailableTimesRequestDto availableTimesRequestDto, CancellationToken cancellationToken);
}
