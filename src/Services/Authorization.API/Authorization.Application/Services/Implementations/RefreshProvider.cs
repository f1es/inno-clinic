using Authorization.Application.Services.Interfaces;
using System.Security.Cryptography;

namespace Authorization.Application.Services.Implementations;

public class RefreshProvider : IRefreshProvider
{
	public string GenerateToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
}
