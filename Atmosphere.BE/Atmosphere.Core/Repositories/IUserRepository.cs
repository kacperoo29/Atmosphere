using System.Linq.Expressions;
using Atmosphere.Core.Models;

namespace Atmosphere.Core.Repositories;

public interface IUserRepository
{
    Task<BaseUser> GetUserAsync(Guid id);
    Task<BaseUser> AddAsync(BaseUser user);
    Task<List<BaseUser>> FindAsync(Expression<Func<BaseUser, bool>> filter);
}