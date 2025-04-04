using Appointment.Application.Services.Interfaces;
using Appointment.Core.Email;
using Appointment.Core.Repositories;
using Appointment.Core.RequestClients;
using Appointment.Infrastructure.Email.Models;
using Shared.Exceptions;

namespace Appointment.Application.Services.Implementations;

public class NotifyService : INotifyService
{
	private readonly IPatientsRequestClient _patientsRequestClient;
	private readonly IAccountRequestClient _accountRequestClient;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IEmailSender _emailSender;

	public NotifyService(
		IPatientsRequestClient patientsRequestClient,
		IUnitOfWork unitOfWork,
		IEmailSender emailSender,
		IAccountRequestClient accountRequestClient)
	{
		_patientsRequestClient = patientsRequestClient;
		_unitOfWork = unitOfWork;
		_emailSender = emailSender;
		_accountRequestClient = accountRequestClient;
	}

	public async Task NotifyAsync(Guid patientId, Guid appointmentId, CancellationToken cancellationToken)
	{
		var patient = await _patientsRequestClient.GetPatientAsync(patientId, cancellationToken);
		if (patient is null)
		{
			throw new NotFoundException(nameof(patient), patientId);
		}
		if (patient.AccountId is null)
		{
			throw new NotFoundException("account", "null");
		}

		var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(appointmentId, cancellationToken);
		if (appointment is null)
		{
			throw new NotFoundException(nameof(appointment), appointmentId);
		}

		var account = await _accountRequestClient.GetAccountAsync(patient.AccountId.Value, cancellationToken);
		if (account is null)
		{
			throw new NotFoundException(nameof(account), patient.AccountId.Value);
		}

		var messageContent = $"We remind you than tomorrow at {appointment.BeginTime} you have an appointment in Inno Clinic";
		var message = new MessageRequestDto([account.Email], "Reminder", messageContent);
		await _emailSender.SendEmailAsync(message, cancellationToken);
	}
}
