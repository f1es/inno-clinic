namespace Appointment.Core.Dto.Request;

public record AvailableTimesRequestDto(DateOnly Date, int Minutes);
