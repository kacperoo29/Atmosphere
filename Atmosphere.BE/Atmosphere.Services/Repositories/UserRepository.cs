namespace Atmosphere.Services.Repositories;

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;

using MongoDB.Driver;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<BaseUser> _users;

    public UserRepository(IMongoCollection<BaseUser> users)
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
