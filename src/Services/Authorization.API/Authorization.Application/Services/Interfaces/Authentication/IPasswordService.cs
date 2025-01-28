using Microsoft.AspNetCore.Identity;

namespace Authorization.Application.Services.Interfaces.Authentication;

public interface IPasswordService
{
    public string Hash(string password);
    public PasswordVerificationResult Verify(string hash, string password);
}
