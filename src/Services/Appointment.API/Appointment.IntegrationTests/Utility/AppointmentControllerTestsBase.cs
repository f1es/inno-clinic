using Appointment.API.Controllers;
using Appointment.Application.Mappers.Implementations;
using Appointment.Application.Mappers.Interfaces;
using Appointment.Application.Services.Implementations;
using Appointment.Application.Services.Interfaces;
using Appointment.Core.Repositories;
using Appointment.Infrastructure.Context;
using Appointment.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Appointment.IntegrationTests;

public abstract class AppointmentControllerTestsBase : IAsyncLifetime
{
	protected readonly PostgreSqlContainer _postgresContainer;
	protected readonly DbContextOptions<AppointmentDbContext> _dbContextOptions;
	protected readonly AppointmentDbContext _context;

	protected readonly IAppointmentsMapper _appointmentsMapper;
	protected readonly IUnitOfWork _unitOfWork;
	protected readonly IAppointmentService _appointmentService;

	protected readonly AppointmentController _appointmentController;

    public AppointmentControllerTestsBase()
    {
		_postgresContainer = new PostgreSqlBuilder().Build();

		_postgresContainer.StartAsync().Wait();

		_dbContextOptions = new DbContextOptionsBuilder<AppointmentDbContext>()
			.UseNpgsql(_postgresContainer.GetConnectionString())
			.Options;

		_context = new AppointmentDbContext(_dbContextOptions);
		_context.Database.Migrate();

		_unitOfWork = new UnitOfWork(_context);
		_appointmentsMapper = new AppointmentsMapper();
		_appointmentService = new AppointmentService(_unitOfWork, _appointmentsMapper);
		_appointmentController = new AppointmentController(_appointmentService);
	}

	public Task InitializeAsync()
	{
		return _postgresContainer.StartAsync();
	}

	public Task DisposeAsync()
	{
		return _postgresContainer.StopAsync();
	}
}
