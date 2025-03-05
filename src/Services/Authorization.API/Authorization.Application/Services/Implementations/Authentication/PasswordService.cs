using Authorization.Application.Services.Interfaces.Authentication;
using Authorization.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Authorization.Application.Services.Implementations.Authentication;

public class PasswordService : IPasswordService
{
    private readonly IPasswordHasher<Account> _passwordHasher;
    public PasswordService(IPasswordHasher<Account> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }
    public string Hash(string password) => _passwordHasher.HashPassword(null, password);
    public PasswordVerificationResult Verify(string hash, string password) => _passwordHasher.VerifyHashedPassword(null, hash, password);
}
