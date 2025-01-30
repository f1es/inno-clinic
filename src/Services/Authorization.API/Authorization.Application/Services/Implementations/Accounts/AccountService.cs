using Authorization.Application.Mappers;
using Authorization.Application.Services.Interfaces.Accounts;
using Authorization.Core.Dto.Request;
using Authorization.Core.Dto.Response;
using Authorization.Core.Models;
using Authorization.Core.Repositories;
using Shared.Exceptions;

namespace Authorization.Application.Services.Implementations.Accounts;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
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
