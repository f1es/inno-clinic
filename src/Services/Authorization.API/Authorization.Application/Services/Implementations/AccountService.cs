using Authorization.Application.Services.Interfaces;
using Authorization.Core.Dto.Request;
using Authorization.Core.Models;
using Authorization.Core.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Authorization.Application.Services.Implementations;

public class AccountService : IAccountService
{
	private readonly IAccountRepository _accountRepository;
	private readonly IPasswordService _passwordHasher;
	private readonly IJwtProvider _jwtProvider;

	public AccountService(
		IAccountRepository accountRepository,
		IPasswordService passwordHasher,
		IJwtProvider jwtProvider)
	{
		_accountRepository = accountRepository;
		_passwordHasher = passwordHasher;
		_jwtProvider = jwtProvider;
	}

	public async Task<string> LoginAsync(LoginAccountRequestDto loginAccountRequestDto)
	{
		var account = await _accountRepository.GetByEmailAsync(loginAccountRequestDto.Email);

		if (account == null)
		{
			// ex 404
		}

		var verificationResult = _passwordHasher.Verify(account.Password, loginAccountRequestDto.Password);

		if (verificationResult == PasswordVerificationResult.Failed)
		{
			// ex 401
		}

		return _jwtProvider.GenerateToken();
	}

	public async Task RegisterAsync(RegisterAccountRequestDto registerAccountRequestDto)
	{
		var passwordHash = _passwordHasher.Hash(registerAccountRequestDto.Password);

		var account = new Account
		{
			Email = registerAccountRequestDto.Email,
			Password = passwordHash,
			PhoneNumber = registerAccountRequestDto.PhoneNumber,
			CreatedAt = DateTime.UtcNow,
			PhotoId = registerAccountRequestDto.PhotoId,
			IsEmailVerified = false,
			CreatedBy = "Annonumys"
		};

		_accountRepository.Create(account);

		await _accountRepository.SaveAsync();
	}


}
