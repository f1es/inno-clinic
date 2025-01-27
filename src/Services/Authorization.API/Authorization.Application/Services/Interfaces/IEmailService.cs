namespace Authorization.Application.Services.Interfaces;

public interface IEmailService
{
	public Task SendEmailAsync();
	public Task VerifyEmailAsync(string verificationToken);
}
