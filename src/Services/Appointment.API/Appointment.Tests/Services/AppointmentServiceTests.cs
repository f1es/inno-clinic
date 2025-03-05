using Appointment.Application.Mappers.Implementations;
using Appointment.Application.Mappers.Interfaces;
using Appointment.Application.Services.Implementations;
using Appointment.Core.Dto.Request;
using Appointment.Core.Repositories;
using AutoFixture;
using Moq;
using Shared.Exceptions;

namespace Appointment.Tests.Services;

public class AppointmentServiceTests
{
	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly IAppointmentsMapper _appointmentsMapper;
	private readonly Fixture _fixture;

	private readonly AppointmentService _appointmentService;

    public AppointmentServiceTests()
    {
		_unitOfWorkMock = new Mock<IUnitOfWork>();
		_appointmentsMapper = new AppointmentsMapper();
		_fixture = new Fixture();
		_fixture.Register(() => DateOnly.FromDateTime(_fixture.Create<DateTime>()));
		_fixture.Register(() => TimeOnly.FromDateTime(_fixture.Create<DateTime>()));

		_appointmentService = new AppointmentService(
			_unitOfWorkMock.Object,
			_appointmentsMapper);
	}

	[Fact]
	public async Task GetAllAsync_Void_ReturnsAppointmentResponseDtoCollection()
	{
		// Arrange 
		var appointmentsCollection = new List<Core.Models.Appointment>
		{
			_fixture.Build<Core.Models.Appointment>().Without(x => x.Result).Create(),
		};
		var responseDtoCollection = _appointmentsMapper.ToResponse(appointmentsCollection);

		_unitOfWorkMock.Setup(x => x.AppointmentRepository.GetAllAsync())
			.ReturnsAsync(appointmentsCollection);

		// Act 
		var result = await _appointmentService.GetAllAsync();

		// Assert

		_unitOfWorkMock.Verify(x => x.AppointmentRepository.GetAllAsync(), Times.Once);

		Assert.NotNull(result);
		Assert.Equivalent(responseDtoCollection, result);
	}


	[Fact]
	public async Task GetByIdAsync_AppointmentId_ReturnsAppointmentResponseDto()
	{
		// Arrange
		var id = Guid.NewGuid();
		var appointment = _fixture.Build<Core.Models.Appointment>().Without(x => x.Result).Create();
		appointment.Id = id;
		var trackChanges = false;

		var appointmentResponse = _appointmentsMapper.ToResponse(appointment);

		_unitOfWorkMock.Setup(x => x.AppointmentRepository.GetByIdAsync(id, trackChanges))
			.ReturnsAsync(appointment);

		// Act
		var result = await _appointmentService.GetByIdAsync(id);

		// Assert
		_unitOfWorkMock.Verify(x => x.AppointmentRepository.GetByIdAsync(id, trackChanges), Times.Once);

		Assert.Equivalent(appointmentResponse, result);
	}

	[Fact]
	public async Task GetByIdAsync_InvalidAppointmentId_ThrowsNotFoundException()
	{
		// Arrange
		var id = new Guid();
		var trackChanges = false;

		_unitOfWorkMock.Setup(x => x.AppointmentRepository.GetByIdAsync(id, trackChanges))
			.ReturnsAsync(It.IsAny<Core.Models.Appointment>());
		// Act
		var function = async () => await _appointmentService.GetByIdAsync(id);

		// Assert
		await Assert.ThrowsAsync<NotFoundException>(function);
	}

	[Fact]
	public async Task CreateAsync_ValidObjectRequest_ReturnsAppointmentResponseDto()
	{
		// Arrange
		_unitOfWorkMock.Setup(x => x.AppointmentRepository.Create(It.IsAny<Core.Models.Appointment>()));
		_unitOfWorkMock.Setup(x => x.SaveAsync());

		var appointmentRequest = _fixture.Create<AppointmentRequestDto>();
		var model = _appointmentsMapper.ToModel(appointmentRequest);
		var response = _appointmentsMapper.ToResponse(model);

		// Act
		var result = await _appointmentService.CreateAsync(appointmentRequest);

		// Assert
		_unitOfWorkMock.Verify(x =>
		x.AppointmentRepository.Create(It.IsAny<Core.Models.Appointment>()), Times.Once);

		Assert.NotNull(result);
		Assert.Equivalent(result, response);
	}

	[Fact]
	public async Task UpdateAsync_AppointmentIdAndValidObjectRequest_ReturnsTask()
	{
		// Arrange
		var appointmentRequest = _fixture.Create<AppointmentRequestDto>();
		var appointment = _fixture.Build<Core.Models.Appointment>().Without(x => x.Result).Create();
		var appointmentAfterUpdate = _appointmentsMapper.ToModel(appointmentRequest);
		appointmentAfterUpdate.Id = appointment.Id;
		var trackChanges = true;

		var id = Guid.NewGuid();
		_unitOfWorkMock.Setup(x => x.AppointmentRepository.GetByIdAsync(id, trackChanges))
			.ReturnsAsync(appointment);

		// Act 
		await _appointmentService.UpdateAsync(id, appointmentRequest);

		// Assert
		_unitOfWorkMock.Verify(x => x.AppointmentRepository.GetByIdAsync(id, trackChanges), Times.Once);

		Assert.Equivalent(appointment, appointmentAfterUpdate);
	}

	[Fact]
	public async Task UpdateAsync_InvalidAppointmentIdAndValidObjectRequest_ThrowsNotFoundException()
	{
		// Arrange
		var appointmentRequest = _fixture.Create<AppointmentRequestDto>();
		var id = new Guid();
		var trackChanges = false;

		_unitOfWorkMock.Setup(x => x.AppointmentRepository.GetByIdAsync(id, trackChanges))
			.ReturnsAsync(It.IsAny<Core.Models.Appointment>());

		// Act
		var function = async () => await _appointmentService.UpdateAsync(id, appointmentRequest);

		// Assert
		await Assert.ThrowsAsync<NotFoundException>(function);
	}

	[Fact]
	public async Task DeleteAsync_AppointmentId_ReturnsTask()
	{
		// Arrange
		var id = Guid.NewGuid();
		var appointment = _fixture.Build<Core.Models.Appointment>().Without(x => x.Result).Create();
		var trackChange = false;

		_unitOfWorkMock.Setup(x => x.AppointmentRepository.GetByIdAsync(id, trackChange))
			.ReturnsAsync(appointment);

		_unitOfWorkMock.Setup(x => x.AppointmentRepository.Delete(appointment));

		// Act 
		await _appointmentService.DeleteAsync(id);

		// Assert
		_unitOfWorkMock.Verify(x => x.AppointmentRepository.GetByIdAsync(id, trackChange), Times.Once);
		_unitOfWorkMock.Verify(x => x.AppointmentRepository.Delete(appointment), Times.Once);
	}

	[Fact]
	public async Task DeleteAsync_InvalidId_ThrowsNotFoundException()
	{
		// Arrange
		var id = new Guid();
		var trackChanges = false;

		_unitOfWorkMock.Setup(x => x.AppointmentRepository.GetByIdAsync(id, trackChanges))
			.ReturnsAsync(It.IsAny<Core.Models.Appointment>());

		// Act 
		var function = async () => await _appointmentService.DeleteAsync(id);

		// Assert
		await Assert.ThrowsAsync<NotFoundException>(function);
	}
}
