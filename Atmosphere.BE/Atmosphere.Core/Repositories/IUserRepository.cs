namespace Atmosphere.Core.Repositories;

using Atmosphere.Core.Models;

public interface IUserRepository
{
    Task<IUser> GetUserAsync(Guid id);
}
