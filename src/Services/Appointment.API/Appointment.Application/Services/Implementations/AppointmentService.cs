using Appointment.Application.Mappers.Interfaces;
using Appointment.Application.Services.Interfaces;
using Appointment.Core.Dto.Request;
using Appointment.Core.Dto.Response;
using Appointment.Core.Repositories;
using Shared.Exceptions;

namespace Appointment.Application.Services.Implementations;

public class AppointmentService : IAppointmentService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAppointmentsMapper _appointmentsMapper;

	public AppointmentService(
		IUnitOfWork unitOfWork, 
		IAppointmentsMapper appointmentsMapper)
	{
		_unitOfWork = unitOfWork;
		_appointmentsMapper = appointmentsMapper;
	}

	public async Task<AppointmentResponseDto> CreateAsync(AppointmentRequestDto appointmentRequestDto)
	{
		var appointment = _appointmentsMapper.ToModel(appointmentRequestDto);

		_unitOfWork.AppointmentRepository.Create(appointment);
		await _unitOfWork.SaveAsync();

		var appointmentResponse = _appointmentsMapper.ToResponse(appointment);

		return appointmentResponse;
	}

	public async Task DeleteAsync(Guid id)
	{
		var appointment = await GetByIdAndCheckIfExist(id);

		_unitOfWork.AppointmentRepository.Delete(appointment);
		await _unitOfWork.SaveAsync();
	}

	public async Task<IEnumerable<AppointmentResponseDto>> GetAllAsync()
	{
		var appointments = await _unitOfWork.AppointmentRepository.GetAllAsync();

		return _appointmentsMapper.ToResponse(appointments);
	}

	public async Task<AppointmentResponseDto> GetByIdAsync(Guid id)
	{
		var appointment = await GetByIdAndCheckIfExist(id);

		return _appointmentsMapper.ToResponse(appointment);
	}

	public async Task UpdateAsync(Guid id, AppointmentRequestDto appointmentRequestDto)
	{
		var appointment = await GetByIdAndCheckIfExist(id, trackChanges: true);

		appointment.ServiceId = appointmentRequestDto.ServiceId;
		appointment.PatientId = appointmentRequestDto.PatientId;
		appointment.DoctorId = appointmentRequestDto.DoctorId;
		appointment.IsApproved = appointmentRequestDto.IsApproved;
		appointment.Date = appointmentRequestDto.Date;
		appointment.Time = appointmentRequestDto.Time;

		await _unitOfWork.SaveAsync();
	}

	private async Task<Core.Models.Appointment> GetByIdAndCheckIfExist(Guid id, bool trackChanges = false)
	{
		var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id, trackChanges);

		return appointment ?? throw new NotFoundException(nameof(appointment), id);
	}
}
