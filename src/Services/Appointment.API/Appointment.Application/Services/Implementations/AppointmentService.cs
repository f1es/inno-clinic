using Appointment.Application.Mappers.Interfaces;
using Appointment.Application.Services.Interfaces;
using Appointment.Core.Dto.Request;
using Appointment.Core.Dto.Response;
using Appointment.Core.Repositories;
using Appointment.Core.RequestClients;
using Shared.Exceptions;

namespace Appointment.Application.Services.Implementations;

public class AppointmentService : IAppointmentService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAppointmentsMapper _appointmentsMapper;
	private readonly IServicesRequestClient _servicesRequestClient;
	private readonly ITimeSlotService _timeSlotService;

	public AppointmentService(
		IUnitOfWork unitOfWork, 
		IAppointmentsMapper appointmentsMapper,
		IServicesRequestClient servicesRequestClient,
		ITimeSlotService timeSlotService)
	{
		_unitOfWork = unitOfWork;
		_appointmentsMapper = appointmentsMapper;
		_servicesRequestClient = servicesRequestClient;
		_timeSlotService = timeSlotService;
	}

	public async Task<AppointmentResponseDto> CreateAsync(AppointmentRequestDto appointmentRequestDto, CancellationToken cancellationToken)
	{
		var appointment = _appointmentsMapper.ToModel(appointmentRequestDto);

		if (!await _timeSlotService.CheckIfSlotAvailableAsync(appointment, cancellationToken))
		{
			throw new BadRequestException($"Time {appointment.BeginTime} - {appointment.EndTime} already reserved or incorrect");
		}

		await CheckIfServiceExistAsync(appointment.ServiceId);

		_unitOfWork.AppointmentRepository.Create(appointment);
		await _unitOfWork.SaveAsync(cancellationToken);

		var appointmentResponse = _appointmentsMapper.ToResponse(appointment);

		return appointmentResponse;
	}

	public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
	{
		var appointment = await GetByIdAndCheckIfExist(id, cancellationToken);

		_unitOfWork.AppointmentRepository.Delete(appointment);
		await _unitOfWork.SaveAsync(cancellationToken);
	}

	public async Task<IEnumerable<AppointmentResponseDto>> GetAllAsync(CancellationToken cancellationToken)
	{
		var appointments = await _unitOfWork.AppointmentRepository.GetAllAsync(cancellationToken);

		return _appointmentsMapper.ToResponse(appointments);
	}

	public async Task<AppointmentResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		var appointment = await GetByIdAndCheckIfExist(id, cancellationToken);

		return _appointmentsMapper.ToResponse(appointment);
	}

	public async Task UpdateAsync(Guid id, AppointmentRequestDto appointmentRequestDto, CancellationToken cancellationToken)
	{
		var appointment = await GetByIdAndCheckIfExist(id, cancellationToken, trackChanges: true);

		await CheckIfServiceExistAsync(appointment.ServiceId);

		_appointmentsMapper.Update(appointmentRequestDto, appointment);

		await _unitOfWork.SaveAsync(cancellationToken);
	}

	private async Task<Core.Models.Appointment> GetByIdAndCheckIfExist(Guid id, CancellationToken cancellationToken, bool trackChanges = false)
	{
		var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id, cancellationToken, trackChanges);

		return appointment ?? throw new NotFoundException(nameof(appointment), id);
	}

	private async Task CheckIfServiceExistAsync(Guid? serviceId)
	{
		if (serviceId == null)
		{
			return;
		}

		if (!await _servicesRequestClient.IsServiceExistAsync(serviceId.Value, CancellationToken.None))
		{
			throw new NotFoundException("service", serviceId.Value);
		}
	}
}
