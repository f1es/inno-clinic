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

namespace Appointment.IntegrationTests.Utility;

public abstract class ResultControllerTestBase : IAsyncLifetime
{
    protected readonly PostgreSqlContainer _postgresContainer;
    protected readonly DbContextOptions<AppointmentDbContext> _dbContextOptions;
    protected readonly AppointmentDbContext _context;

    protected readonly IResultsMapper _resultsMapper;
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IResultService _resultService;

    protected readonly ResultController _resultController;

    public ResultControllerTestBase()
    {
        _postgresContainer = new PostgreSqlBuilder().Build();

        _postgresContainer.StartAsync().Wait();

        _dbContextOptions = new DbContextOptionsBuilder<AppointmentDbContext>()
            .UseNpgsql(_postgresContainer.GetConnectionString())
            .Options;

        _context = new AppointmentDbContext(_dbContextOptions);
        _context.Database.Migrate();

        _unitOfWork = new UnitOfWork(_context);
        _resultsMapper = new ResultsMapper();
        _resultService = new ResultService(_unitOfWork, _resultsMapper);
        _resultController = new ResultController(_resultService);
    }

    public Task DisposeAsync()
    {
        return _postgresContainer.StopAsync();
    }

    public Task InitializeAsync()
    {
        return _postgresContainer.StartAsync();
    }
}
