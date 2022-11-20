using Atmosphere.Application.Services;
using Atmosphere.Application.Services;
using Atmosphere.Core;
using Atmosphere.Core.Consts;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Atmosphere.Services.Auth;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepo;

    public UserService(
        IUserRepository userRepo,
        ITokenService tokenService,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _userRepo = userRepo;
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BaseUser> CreateUserAsync(BaseUser user)
    {
        var users = await _userRepo.FindAsync(u => u.Username == user.Username);
        if (!users.IsNullOrEmpty())
            throw new Exception($"User with identifier {user.Username} already exists.");

        return await _userRepo.AddAsync(user);
    }

    public async Task<BaseUser> GetByCredentialsAsync(string identifier, string key)
    {
        var user = (await _userRepo.FindAsync(u => u.Username == identifier)).SingleOrDefault();
        if (user != null && PasswordUtil.VerifyPassword(key, user.Password))
        {
            return user;
        }

        throw new UnauthorizedAccessException("Invalid credentials");
    }

    public async Task<BaseUser> GetByTokenAsync(string token)
    {
        var claims = await _tokenService.GetClaims(token);
        var userId = claims.First(c => c.Type == AtmosphereClaimTypes.UserId).Value;

        return await _userRepo.GetUserAsync(Guid.Parse(userId));
    }

    public async Task<BaseUser?> GetCurrentAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == AtmosphereClaimTypes.UserId)
            ?.Value;
        if (userId == null)
            return null;

        return await _userRepo.GetUserAsync(Guid.Parse(userId));
    }
}
