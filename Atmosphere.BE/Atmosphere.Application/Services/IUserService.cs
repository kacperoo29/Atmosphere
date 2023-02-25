using Atmosphere.Core.Models;

namespace Atmosphere.Application.Services;

public interface IUserService
{
    Task<BaseUser> GetByCredentialsAsync(string identifier, string key);
    Task<BaseUser?> GetCurrentAsync();
    Task<BaseUser> CreateUserAsync(BaseUser user);
}