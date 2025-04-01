using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Appointment.IntegrationTests.Utility.WebFactory;

public class TestAuthHandler : AuthenticationHandler<TestAuthHandlerOptions>
{
	public const string Role = "receptionist";

	public const string AuthenticationScheme = "Test";
	private readonly string _defaultRole;

	public TestAuthHandler(
		IOptionsMonitor<TestAuthHandlerOptions> options,
		ILoggerFactory logger,
		UrlEncoder encoder,
		ISystemClock clock) : base(options, logger, encoder, clock)
	{
		_defaultRole = options.CurrentValue.DefaultRole;
	}

	protected override Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Test user") };

		if (Context.Request.Headers.TryGetValue(Role, out var role))
		{
			claims.Add(new Claim(ClaimTypes.Role, role[0]));
		}
		else
		{
			claims.Add(new Claim(ClaimTypes.Role, _defaultRole));
		}

		var identity = new ClaimsIdentity(claims, AuthenticationScheme);
		var principal = new ClaimsPrincipal(identity);
		var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

		var result = AuthenticateResult.Success(ticket);

		return Task.FromResult(result);
	}
}