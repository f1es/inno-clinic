using Appointment.Core.RequestClients;
using Appointment.Infrastructure.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Testcontainers.PostgreSql;

namespace Appointment.IntegrationTests.Utility.WebFactory;

public class WebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder().Build();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
			services.Configure<TestAuthHandlerOptions>(options =>
            {
                options.DefaultRole = "receptionist";
			});

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
				options.DefaultScheme = TestAuthHandler.AuthenticationScheme;
				options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
			}).AddScheme<TestAuthHandlerOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { });

			var dbDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppointmentDbContext>));

            if (dbDescriptor is not null)
            {
                services.Remove(dbDescriptor);
            }

            var requestClientDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IServicesRequestClient));

            if (requestClientDescriptor is not null)
            {
                services.Remove(requestClientDescriptor);
            }

            services.AddDbContext<AppointmentDbContext>(options =>
            {
                options.UseNpgsql(_dbContainer.GetConnectionString());
            });

            services.AddScoped<IServicesRequestClient, ServicesRequestClientMock>();

            var provider = services.BuildServiceProvider();

            using (var scope = provider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppointmentDbContext>();
                context.Database.Migrate();
            }

            builder.UseEnvironment("Development");
        });
    }

    public Task InitializeAsync()
    {
        return _dbContainer.StartAsync();
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}
