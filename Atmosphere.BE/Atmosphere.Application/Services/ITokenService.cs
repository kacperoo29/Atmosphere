namespace Atmosphere.Application.Services;

public interface ITokenService
{
    Task<string> GenerateToken(Guid userId, string key);
    Task<bool> ValidateToken(string token);
}
