using Atmosphere.Core.Models;

namespace Atmosphere.Application.Services;

public interface ITokenService
{
    Task<string> GenerateToken(IUser user);
    Task<bool> ValidateToken(string token);
}
