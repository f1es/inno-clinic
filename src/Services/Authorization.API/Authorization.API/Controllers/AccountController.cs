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

	/// <summary>
	/// Registration account method
	/// </summary>
	/// <param name="registerAccountRequestDto">Registration account request data transfer object</param>
	/// <returns></returns>
	[HttpPost("register")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Register(RegisterAccountRequestDto registerAccountRequestDto)
	{
		await _accountService.RegisterAsync(registerAccountRequestDto);

		return Ok();
	}

	/// <summary>
	/// Login account method
	/// </summary>
	/// <param name="loginAccountRequestDto">Login account request data transfer object</param>
	/// <returns></returns>
	[HttpPost("login")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Login(LoginAccountRequestDto loginAccountRequestDto)
	{
		var token = await _accountService.LoginAsync(loginAccountRequestDto);

		Response.Cookies.Append("sec", token);

		return Ok(token);
	}

	/// <summary>
	/// Get accounts method
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetAll()
	{
		var accounts = await _accountService.GetAllAsync();

		return Ok(accounts);
	}

	/// <summary>
	/// Get account by id method
	/// </summary>
	/// <param name="id">Account's unique identifier</param>
	/// <returns></returns>
	[HttpGet("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetById(Guid id)
	{
		var account = await _accountService.GetByIdAsync(id);

		return Ok(account);
	}

	/// <summary>
	/// Delete account method
	/// </summary>
	/// <param name="id">Account's unique identifier</param>
	/// <returns></returns>
	[HttpDelete("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _accountService.DeleteAsync(id);

		return NoContent();
	}

	/// <summary>
	/// Update account method
	/// </summary>
	/// <param name="id">Account's unique identifier</param>
	/// <param name="updateAccountRequestDto">Update account request data transfer object</param>
	/// <returns></returns>
	[HttpPut("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Update(Guid id, UpdateAccountRequestDto updateAccountRequestDto)
	{
		await _accountService.UpdateAsync(id, updateAccountRequestDto);

		return NoContent();
	}

}
