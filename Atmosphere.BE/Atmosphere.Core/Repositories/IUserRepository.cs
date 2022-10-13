namespace Atmosphere.Core.Repositories;

using Atmosphere.Core.Models;

public interface IUserRepository
{
    Task<User> GetUserAsync(Guid id);
    Task<User> GetByCredentialsAsync(string identifier, string key);
}
