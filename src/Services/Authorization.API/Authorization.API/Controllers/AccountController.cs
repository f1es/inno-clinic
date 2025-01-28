using Authorization.Application.Services.Interfaces.Accounts;
using Authorization.Application.Services.Interfaces.Authentication;
using Authorization.Application.Services.Interfaces.Email;
using Authorization.Application.Utility;
using Authorization.Core.Dto.Request;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.API.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountController : ControllerBase
{
	private readonly IAccountService _accountService;
	private readonly IAccessService _accessService;
	private readonly IRegistrationService _registrationService;
	private readonly IEmailVerificationService _emailVerificationService;

	public AccountController(
		IAccountService accountService,
		IAccessService accessService,
		IRegistrationService registrationService,
		IEmailVerificationService emailVerificationService)
	{
		_accountService = accountService;
		_accessService = accessService;
		_registrationService = registrationService;
		_emailVerificationService = emailVerificationService;
	}

	/// <summary>
	/// Registration account method
	/// </summary>
	/// <param name="registerAccountRequestDto">Registration account request data transfer object</param>
	/// <returns></returns>
	[HttpPost("register")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Register(RegisterAccountRequestDto registerAccountRequestDto)
	{
		await _registrationService.RegisterAsync(registerAccountRequestDto);

		return Ok();
	}

	/// <summary>
	/// Login account method
	/// </summary>
	/// <param name="loginAccountRequestDto">Login account request data transfer object</param>
	/// <returns></returns>
	[HttpPost("login")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Login(LoginAccountRequestDto loginAccountRequestDto)
	{
		var tokens = await _accessService.LoginAsync(loginAccountRequestDto);

		Response.Cookies.Append("sec", tokens.AccessToken);
		Response.Cookies.Append("ref", tokens.RefreshToken);

		return Ok(tokens);
	}

	/// <summary>
	/// Get accounts method
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[Produces("application/json")]
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
	[Produces("application/json")]
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
	[Produces("application/json")]
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
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Update(Guid id, UpdateAccountRequestDto updateAccountRequestDto)
	{
		await _accountService.UpdateAsync(id, updateAccountRequestDto);

		return NoContent();
	}

	/// <summary>
	/// Endpoint for email verification
	/// </summary>
	/// <param name="token">Email verification token, send to email when user register</param>
	/// <returns></returns>
	[HttpGet("email-verification/")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> VerifyEmail([FromQuery] string token)
	{
		await _emailVerificationService.VerifyEmailAsync(token);

		return Ok();
	}

	/// <summary>
	/// Refresh access and refresh tokens from cookies
	/// </summary>
	/// <returns></returns>
	[HttpPost("refresh")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Refresh()
	{
		var tokens = new Tokens();
		tokens.RefreshToken = Request.Cookies["ref"];
		tokens.AccessToken = Request.Cookies["sec"];

		tokens = await _accessService.RefreshAsync(tokens);

		var cookieOptions = new CookieOptions
		{
			HttpOnly = true,
			Expires = DateTime.UtcNow.AddDays(31),
		};

		Response.Cookies.Append("ref", tokens.RefreshToken, cookieOptions);
		Response.Cookies.Append("sec", tokens.AccessToken, cookieOptions);

		return Ok(tokens);
	}

	/// <summary>
	/// Remove refresh token from data base
	/// </summary>
	/// <returns></returns>
	[HttpPost("revoke")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Revoke()
	{
		var tokens = new Tokens();
		tokens.RefreshToken = Request.Cookies["ref"];
		tokens.AccessToken = Request.Cookies["sec"];

		await _accessService.RevokeAsync(tokens);

		return NoContent();
	}
}
