using Authorization.Application.Services.Interfaces.TokenProviers;
using System.Security.Cryptography;

namespace Authorization.Application.Services.Implementations.TokenProviders;

public class RefreshProvider : IRefreshProvider
{
    public string GenerateToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
}
