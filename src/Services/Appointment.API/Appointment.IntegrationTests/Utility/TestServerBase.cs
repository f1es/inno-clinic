using Appointment.Infrastructure.Context;
using Appointment.IntegrationTests.Utility.WebFactory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using System.Data.Common;
using System.Net.Http.Headers;

namespace Appointment.IntegrationTests.Utility;

public class TestServerBase : IClassFixture<WebAppFactory>, IAsyncLifetime
{
    private readonly IServiceScope _scope;
    protected readonly HttpClient _httpClient;
    protected readonly AppointmentDbContext _context;
	protected Respawner _respawner = default!;
	protected DbConnection _connection = default!;
	protected TestServerBase(WebAppFactory testsWebAppFactory)
    {
        _scope = testsWebAppFactory.Services.CreateScope();
        _httpClient = testsWebAppFactory.CreateDefaultClient();
        _context = _scope.ServiceProvider.GetRequiredService<AppointmentDbContext>();

		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
	}

	public async Task InitializeAsync()
	{
		_connection = _context.Database.GetDbConnection();
		await _connection.OpenAsync();
		_respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
		{
			SchemasToInclude = ["public"],
			DbAdapter = DbAdapter.Postgres
		});
	}

	public async Task DisposeAsync()
	{
		await _respawner.ResetAsync(_connection);
	}
}
