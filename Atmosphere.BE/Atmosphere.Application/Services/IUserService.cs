using Atmosphere.Core.Models;

namespace Atmoshpere.Application.Services;

public interface IUserService
{
    Task<BaseUser> GetByCredentialsAsync(string identifier, string key);
    Task<BaseUser> GetByTokenAsync(string token);
    Task<BaseUser?> GetCurrent();
    Task<BaseUser> CreateUser(BaseUser user);
} 