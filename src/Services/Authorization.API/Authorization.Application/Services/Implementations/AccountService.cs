using Authorization.Application.Mappers;
using Authorization.Application.Services.Interfaces;
using Authorization.Application.Extensions;
using Authorization.Core.Dto.Request;
using Authorization.Core.Dto.Response;
using Authorization.Core.Models;
using Authorization.Core.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Authorization.Application.Services.Implementations;

public class AccountService : IAccountService
{
	private readonly IAccountRepository _accountRepository;
	private readonly IPasswordService _passwordHasher;
	private readonly IJwtProvider _jwtProvider;
	private readonly IValidator<RegisterAccountRequestDto> _validator;

	public AccountService(
		IAccountRepository accountRepository,
		IPasswordService passwordHasher,
		IJwtProvider jwtProvider,
		IValidator<RegisterAccountRequestDto> validator)
	{
		_accountRepository = accountRepository;
		_passwordHasher = passwordHasher;
		_jwtProvider = jwtProvider;
		_validator = validator;
	}

	public async Task<string> LoginAsync(LoginAccountRequestDto loginAccountRequestDto)
	{
		var account = await _accountRepository.GetByEmailAsync(loginAccountRequestDto.Email);

		if (account == null)
		{
			throw new Exception("404");
			// ex 404
		}

		if (!account.IsEmailVerified)
		{
			throw new Exception("401");
		}

		var verificationResult = _passwordHasher.Verify(account.Password, loginAccountRequestDto.Password);

		if (verificationResult == PasswordVerificationResult.Failed)
		{
			throw new Exception("401");
			// ex 401
		}

		return _jwtProvider.GenerateToken();
	}

	public async Task RegisterAsync(RegisterAccountRequestDto registerAccountRequestDto)
	{
		var passwordHash = _passwordHasher.Hash(registerAccountRequestDto.Password);

		var validationResult = await _validator.ValidateAsync(registerAccountRequestDto);

		if (!validationResult.IsValid)
		{
			throw new Exception($"{validationResult.GetErrors()}");
		}

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

	public async Task<IEnumerable<AccountResponseDto>> GetAllAsync()
	{
		var accounts = await _accountRepository.GetAllAsync();
		var accountsDto = new List<AccountResponseDto>();
        foreach (var acc in accounts)
        {
			var accountDto = acc.ToResponseDto();

			accountsDto.Add(accountDto);
        }

		return accountsDto;
    }

	public async Task<AccountResponseDto> GetByIdAsync(Guid id) 
	{
		var account = await _accountRepository.GetByIdAsync(id);

		return account.ToResponseDto();
	}

	public async Task DeleteAsync(Guid id)
	{
		var account = await _accountRepository.GetByIdAsync(id);

		if (account == null)
		{
			// 404 ex
		}

		_accountRepository.Delete(account);

		await _accountRepository.SaveAsync();
	}

	public async Task UpdateAsync(Guid id, UpdateAccountRequestDto updateAccountRequestDto)
	{
		var account = await _accountRepository.GetByIdAsync(id, trackChanges: true);

		if (account == null)
		{
			// 404 ex
		}

		account.PhoneNumber = updateAccountRequestDto.PhoneNumber;
		account.PhotoId = updateAccountRequestDto.PhotoId;

		await _accountRepository.SaveAsync();
	}
}
