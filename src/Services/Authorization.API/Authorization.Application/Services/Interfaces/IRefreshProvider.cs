namespace Authorization.Application.Services.Interfaces;

public interface IRefreshProvider
{
	public string GenerateToken();
}
