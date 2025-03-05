namespace Authorization.Application.Services.Interfaces.TokenProviers;

public interface IRefreshProvider
{
    public string GenerateToken();
}
