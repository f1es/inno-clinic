using Authorization.Application.Options;
using Authorization.Application.Services.Interfaces.JWT;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authorization.Application.Services.Implementations.JWT;

public class JwtProvider : IJwtProvider
{
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly JwtTokenOptions _jwtTokenOptions;
    public JwtProvider(
        JwtSecurityTokenHandler tokenHandler,
        IOptions<JwtTokenOptions> jwtOptions)
    {
        _tokenHandler = tokenHandler;
        _jwtTokenOptions = jwtOptions.Value;
    }

    public string GenerateToken(string key, int lifeTimeHours, ClaimsIdentity claims)
    {
        var symmetricKey = ReadKey(key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddHours(lifeTimeHours),
            SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtTokenOptions.Issuer,
		};

        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }

    public async Task<bool> VerifyEmailTokenAsync(string token, string key, string email)
    {
        var verificationResult = await VerifyTokenAsync(token, key, validateLifetime: false);

        var jwt = _tokenHandler.ReadJwtToken(token);
        var isEmailEquals = jwt.Claims.First(x => x.Type == "email").Value == email;

        return verificationResult && isEmailEquals;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string key)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateLifetime = false
        };

        SecurityToken securityToken;

        var principal = _tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

	public async Task<bool> VerifyAccessTokenAsync(string token, string key) => await VerifyTokenAsync(token, key);

	public JwtSecurityToken ReadToken(string token) => _tokenHandler.ReadJwtToken(token);

	private SymmetricSecurityKey ReadKey(string key) => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

    private async Task<bool> VerifyTokenAsync(string token, string key, bool validateLifetime = true)
    {
        var symmetricKey = ReadKey(key);
        var validationParameters = new TokenValidationParameters
        {
            ValidIssuer = _jwtTokenOptions.Issuer,
            ValidateAudience = false,
            ValidateLifetime = validateLifetime,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = symmetricKey
        };

        var result = await _tokenHandler.ValidateTokenAsync(token, validationParameters);

        return result.IsValid;
    }
}
