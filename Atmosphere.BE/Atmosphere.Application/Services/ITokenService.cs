using System.Security.Claims;

using Atmosphere.Core.Models;
using Microsoft.IdentityModel.Tokens;

namespace Atmosphere.Application.Services;

public interface ITokenService
{
    Task<string> GenerateToken(BaseUser user);
    Task<(bool, SecurityToken?)> ValidateToken(string token);
    Task<List<Claim>> GetClaims(string token);
}
