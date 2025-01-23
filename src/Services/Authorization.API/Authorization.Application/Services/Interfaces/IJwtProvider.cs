using Authorization.Core.Dto.Request;

namespace Authorization.Application.Services.Interfaces;

public interface IJwtProvider
{
	public string GenerateToken();
}
