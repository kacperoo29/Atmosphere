using Atmosphere.Core.Models;

namespace Atmosphere.Application.Services;

public interface ITokenService
{
    Task<string> GenerateToken(User user);
    Task<bool> ValidateToken(string token);
}
