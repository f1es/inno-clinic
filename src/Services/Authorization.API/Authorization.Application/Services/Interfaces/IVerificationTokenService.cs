namespace Authorization.Application.Services.Interfaces;

public interface IVerificationTokenService
{
	public string GenerateToken();
	public bool VerifyToken(string token);
}
