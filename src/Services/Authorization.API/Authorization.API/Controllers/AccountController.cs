using Authorization.Application.Services.Interfaces;
using Authorization.Core.Dto.Request;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.API.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountController : ControllerBase
{
	private readonly IAccountService _accountService;

	public AccountController(IAccountService accountService)
	{
		_accountService = accountService;
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register(RegisterAccountRequestDto registerAccountRequestDto)
	{
		await _accountService.RegisterAsync(registerAccountRequestDto);

		return NoContent();
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login(LoginAccountRequestDto loginAccountRequestDto)
	{
		var token = await _accountService.LoginAsync(loginAccountRequestDto);

		return Ok(token);
	}

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var accounts = await _accountService.GetAllAsync();

		return Ok(accounts);
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetById(Guid id)
	{
		var account = await _accountService.GetByIdAsync(id);

		return Ok(account);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _accountService.DeleteAsync(id);

		return NoContent();
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Update(Guid id, UpdateAccountRequestDto updateAccountRequestDto)
	{
		await _accountService.UpdateAsync(id, updateAccountRequestDto);

		return NoContent();
	}

}
