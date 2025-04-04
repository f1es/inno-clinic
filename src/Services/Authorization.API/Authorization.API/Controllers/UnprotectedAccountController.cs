using Authorization.Application.Services.Interfaces.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.API.Controllers;

[ApiController]
[Route("api/unprotected/accounts")]
[AllowAnonymous]
public class UnprotectedAccountController : ControllerBase
{
	private readonly IAccountService _accountService;

	public UnprotectedAccountController(IAccountService accountService)
	{
		_accountService = accountService;
	}

	/// <summary>
	/// Get account by id method
	/// </summary>
	/// <param name="id">Account's unique identifier</param>
	/// <returns></returns>
	[HttpGet("{id:guid}")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetByIdUnprotected(Guid id)
	{
		var account = await _accountService.GetByIdAsync(id);

		return Ok(account);
	}
}
