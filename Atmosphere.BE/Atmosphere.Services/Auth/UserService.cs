using System.Threading.Tasks;
using Atmoshpere.Application.Services;
using Atmosphere.Application.Services;
using Atmosphere.Core;
using Atmosphere.Core.Consts;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Atmoshpere.Services.Auth;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IUserRepository userRepo, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
    {
        _userRepo = userRepo;
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BaseUser> CreateUser(BaseUser user)
    {
        var users = await this._userRepo.FindAsync(u => u.Identifier == user.Identifier);
        if (!users.IsNullOrEmpty())
        {
            throw new Exception($"User with identifier {user.Identifier} already exists.");
        }

        return await this._userRepo.AddAsync(user);
    }

    public async Task<BaseUser> GetByCredentialsAsync(string identifier, string key)
    {
        var user = (await this._userRepo.FindAsync(u => u.Identifier == identifier)).Single();
        if (user != null && PasswordUtil.VerifyPassword(key, user.Password))
        {
            return user;
        }

        throw new Exception("Invalid credentials");
    }

    public async Task<BaseUser> GetByTokenAsync(string token)
    {
        var claims = await this._tokenService.GetClaims(token);
        var userId = claims.First(c => c.Type == AtmosphereClaimTypes.UserId).Value;
        
        return await this._userRepo.GetUserAsync(Guid.Parse(userId));
    }

    public async Task<BaseUser?> GetCurrent()
    {
        var userId = this._httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == AtmosphereClaimTypes.UserId)?.Value;
        if (userId == null)
        {
            return null;
        }

        return await this._userRepo.GetUserAsync(Guid.Parse(userId));
    }
}
