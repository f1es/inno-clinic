namespace Authorization.Application.Services.Interfaces.Email;

public interface IEmailVerificationService
{
    public Task VerifyEmailAsync(string token);
}
