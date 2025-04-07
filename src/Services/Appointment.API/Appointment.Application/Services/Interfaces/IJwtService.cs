namespace Appointment.Application.Services.Interfaces;

public interface IJwtService
{
	public Guid GetAccountId(string jwt);
}
