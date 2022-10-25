namespace Atmosphere.Services.Auth;

using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Atmosphere.Application.Services;
using Atmosphere.Core.Consts;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class JwtTokenProvider : ITokenService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public JwtTokenProvider(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<string> GenerateToken(BaseUser user)
    {
        var claims = user.GetClaims();
        var config = _configuration.GetSection("JWT");
        var expirationFormats = config.GetSection("ExpirationFormats").Get<string[]>();
        var expiration = TimeSpan.ParseExact(config["Expiration"], expirationFormats, CultureInfo.InvariantCulture);

        var token = new JwtSecurityToken(
            issuer: config["Issuer"],
            audience: config["Audience"],
            claims: claims,
            expires: DateTime.Now.Add(expiration),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["SecretKey"])),
                SecurityAlgorithms.HmacSha256));

        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<List<Claim>> GetClaims(string token)
    {
        var (valid, jwt) = await this.ValidateToken(token);
        if (!valid)
        {
            return new List<Claim>();
        }

        return (jwt as JwtSecurityToken)?.Claims.ToList() ?? new List<Claim>();
    }

    public async Task<(bool, SecurityToken?)> ValidateToken(string token)
    {
        var config = _configuration.GetSection("JWT");
        var tokenHandler = new JwtSecurityTokenHandler();
        var result = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["SecretKey"]))
        });

        if (!result.IsValid)
        {
            return (false, null);
        }

        var jwtToken = result.SecurityToken as JwtSecurityToken;
        if (jwtToken == null)
        {
            return (false, null);
        }

        var userId = jwtToken.Claims.First(c => c.Type == AtmosphereClaimTypes.UserId).Value;
        var user = await _userRepository.GetUserAsync(Guid.Parse(userId));

        return (user != null, jwtToken);
    }
}