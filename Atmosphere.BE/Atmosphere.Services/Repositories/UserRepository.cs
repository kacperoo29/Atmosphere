using System.Linq.Expressions;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using MongoDB.Driver;

namespace Atmosphere.Services.Repositories;

public class UserRepository : BaseRepository<BaseUser>, IUserRepository
{
    private readonly IMongoCollection<BaseUser> _users;

    public UserRepository(IMongoCollection<BaseUser> users)
        : base(users)
    {
        _users = users;
    }

    public async Task<BaseUser> AddAsync(BaseUser user)
    {
        return await _users.InsertOneAsync(user).ContinueWith(_ => user);
    }

    public async Task<List<BaseUser>> FindAsync(Expression<Func<BaseUser, bool>> filter)
    {
        return await (await _users.FindAsync(filter)).ToListAsync();
    }

    public async Task<BaseUser> GetUserAsync(Guid id)
    {
        return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
    }
}