using Microsoft.AspNetCore.Authentication;

namespace Appointment.IntegrationTests.Utility.WebFactory;

public class TestAuthHandlerOptions : AuthenticationSchemeOptions
{
	public string DefaultRole { get; set; } = null!;
}
