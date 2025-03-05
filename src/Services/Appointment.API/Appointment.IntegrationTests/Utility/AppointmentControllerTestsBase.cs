using Appointment.API.Controllers;
using Appointment.Application.Mappers.Implementations;
using Appointment.Application.Mappers.Interfaces;
using Appointment.Application.Services.Implementations;
using Appointment.Application.Services.Interfaces;
using Appointment.Core.Repositories;
using Appointment.Core.RequestClients;
using Appointment.Infrastructure.Context;
using Appointment.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Testcontainers.PostgreSql;

namespace Appointment.IntegrationTests.Utility;

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
        
        var serviceRequestClientMock = new Mock<IServicesRequestClient>();
        serviceRequestClientMock.Setup(x => x.IsServiceExistAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(true);

        _appointmentService = new AppointmentService(_unitOfWork, _appointmentsMapper, serviceRequestClientMock.Object);
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
