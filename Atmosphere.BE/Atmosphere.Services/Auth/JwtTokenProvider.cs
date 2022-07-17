namespace Atmosphere.Services.Auth;

using System.IdentityModel.Tokens.Jwt;
using System.Text;

using Atmosphere.Application.Services;
using Atmosphere.Core.Repositories;

using Microsoft.IdentityModel.Tokens;

public class JwtTokenProvider : ITokenService
{
    private readonly IUserRepository _userRepository;

    public JwtTokenProvider(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<string> GenerateToken(Guid userId, string key)
    {
        var user = await _userRepository.GetUserAsync(userId);
        var claims = user.GetClaims();
        
        var token = new JwtSecurityToken(
            issuer: "Atmosphere",
            audience: "Atmosphere",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256));
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<bool> ValidateToken(string token)
    {
        throw new NotImplementedException();
    }
}