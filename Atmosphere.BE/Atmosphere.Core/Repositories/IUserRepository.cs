namespace Atmosphere.Core.Repositories;

using System.Linq.Expressions;
using Atmosphere.Core.Models;

public interface IUserRepository
{
    Task<BaseUser> GetUserAsync(Guid id);
    Task<BaseUser> AddAsync(BaseUser user);
    Task<List<BaseUser>> FindAsync(Expression<Func<BaseUser, bool>> filter);
}
