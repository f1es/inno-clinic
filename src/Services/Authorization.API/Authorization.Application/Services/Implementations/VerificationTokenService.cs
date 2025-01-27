using Authorization.Application.Services.Interfaces;
using System.Security.Cryptography;

namespace Authorization.Application.Services.Implementations;

public class VerificationTokenService : IVerificationTokenService
{
	public string GenerateToken() => SHA256
		.HashData(Guid.NewGuid().ToByteArray())
		.ToString();

	public bool VerifyToken(string token)
	{
		throw new NotImplementedException();
	}
}
