using Authorization.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Authorization.Application.Services.Interfaces;

public interface IPasswordService
{
	public string Hash(string password);
	public PasswordVerificationResult Verify(string hash, string password);
}
