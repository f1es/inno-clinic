using Authorization.Core.Dto.Response;
using Authorization.Core.Models;
using System.Runtime.CompilerServices;

namespace Authorization.Application.Mappers;

public static class AccountMappers
{
	public static AccountResponseDto ToResponseDto(this Account account) => 
		new AccountResponseDto(
			account.Id,
			account.Email,
			account.PhoneNumber,
			account.IsEmailVerified,
			account.CreatedBy,
			account.CreatedAt,
			account.UpdatedBy,
			account.UpdatedAt,
			account.PhotoId);
}
