using Appointment.Application.Services.Interfaces;
using Appointment.Core.Repositories;
using Quartz;

namespace Appointment.Application.Jobs;

public class AppointmentNotificationJob : IJob
{
	private readonly INotifyService _notificationService;
	private readonly IUnitOfWork _unitOfWork;

	public AppointmentNotificationJob(INotifyService notificationService, IUnitOfWork unitOfWork)
	{
		_notificationService = notificationService;
		_unitOfWork = unitOfWork;
	}

	public async Task Execute(IJobExecutionContext context)
	{
		var jobData = context.MergedJobDataMap;

		var tomorrow = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
		var appointments = await _unitOfWork.AppointmentRepository.GetAllByDayAsync(tomorrow, context.CancellationToken);

		foreach (var appointment in appointments)
		{
			if (appointment.PatientId is null)
			{
				continue;
			}

			await _notificationService.NotifyAsync(appointment.PatientId.Value, appointment.Id, context.CancellationToken);
		}
	}
}
