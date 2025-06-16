using Authorization.Application.Mappers;
using Authorization.Application.Options;
using Authorization.Application.Services.Interfaces.Accounts;
using Authorization.Application.Services.Interfaces.JWT;
using Authorization.Core.Dto.Request;
using Authorization.Core.Dto.Response;
using Authorization.Core.Models;
using Authorization.Core.Repositories;
using Microsoft.Extensions.Options;
using Shared.Exceptions;

namespace Authorization.Application.Services.Implementations.Accounts;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly JwtOptions _jwtOptions;

    public AccountService(IAccountRepository accountRepository, IJwtProvider jwtProvider, IOptions<JwtOptions> jwtOptions)
    {
        _accountRepository = accountRepository;
        _jwtProvider = jwtProvider;
    public async Task<IEnumerable<AccountResponseDto>> GetAllAsync()
    {
        var accounts = await _accountRepository.GetAllAsync();
        var accountsDto = accounts.Select(acc => acc.ToResponseDto());

        return accountsDto;
    }

    public async Task<AccountResponseDto> GetByIdAsync(Guid id)
    {
        var account = await _accountRepository.GetByIdAsync(id);

        return account.ToResponseDto();
    }

    public async Task<AccountResponseDto> GetByJwtAsync(string accessToken)
    {

        return await GetByIdAsync(accountId);
    }

    public async Task DeleteAsync(Guid id)
    {
        var account = await _accountRepository.GetByIdAsync(id);

        AccountNullCheck(account, id);

        _accountRepository.Delete(account);

        await _accountRepository.SaveAsync();
    }

    public async Task UpdateAsync(Guid id, UpdateAccountRequestDto updateAccountRequestDto)
    {
        var account = await _accountRepository.GetByIdAsync(id, trackChanges: true);
        
        AccountNullCheck(account, id);

        account.PhoneNumber = updateAccountRequestDto.PhoneNumber;
        account.PhotoId = updateAccountRequestDto.PhotoId;

        await _accountRepository.SaveAsync();
    }

    private Account AccountNullCheck(Account account, Guid id) => account ?? throw new NotFoundException(nameof(account), id);
}
