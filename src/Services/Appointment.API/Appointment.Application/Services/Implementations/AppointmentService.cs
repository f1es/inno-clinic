using Appointment.Application.Mappers.Interfaces;
using Appointment.Application.Services.Interfaces;
using Appointment.Core.Dto.Request;
using Appointment.Core.Dto.Response;
using Appointment.Core.GrpcClients;
using Appointment.Core.Repositories;
using Shared.Exceptions;

namespace Appointment.Application.Services.Implementations;

public class AppointmentService : IAppointmentService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IAppointmentsMapper _appointmentsMapper;
	private readonly IServiceGrpcClient _serviceGrpcClient;

	public AppointmentService(
		IUnitOfWork unitOfWork, 
		IAppointmentsMapper appointmentsMapper,
		IServiceGrpcClient serviceGrpcClient)
	{
		_unitOfWork = unitOfWork;
		_appointmentsMapper = appointmentsMapper;
		_serviceGrpcClient = serviceGrpcClient;
	}

	public async Task<AppointmentResponseDto> CreateAsync(AppointmentRequestDto appointmentRequestDto)
	{
		var appointment = _appointmentsMapper.ToModel(appointmentRequestDto);

		await CheckIfServiceExistAsync(appointment.ServiceId);

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

		_appointmentsMapper.Update(appointmentRequestDto, appointment);

		await _unitOfWork.SaveAsync();
	}

	private async Task<Core.Models.Appointment> GetByIdAndCheckIfExist(Guid id, bool trackChanges = false)
	{
		var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id, trackChanges);

		return appointment ?? throw new NotFoundException(nameof(appointment), id);
	}

	private async Task CheckIfServiceExistAsync(Guid? serviceId)
	{
		if (serviceId == null)
		{
			return;
		}

		if (!await _serviceGrpcClient.IsServiceExist(serviceId.Value, CancellationToken.None))
		{
			throw new NotFoundException("service", serviceId.Value);
		}
	}
}
