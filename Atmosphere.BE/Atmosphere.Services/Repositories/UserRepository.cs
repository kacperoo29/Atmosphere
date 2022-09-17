namespace Atmosphere.Services.Repositories;

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;

using MongoDB.Driver;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<IUser> _users;

    public UserRepository(IMongoCollection<IUser> users)
    {
        _users = users;
    }

    public async Task<IUser> GetByCredentialsAsync(string identifier, string key)
    {
        var user = (await _users.FindAsync(u => u.GetIdentifier() == identifier)).FirstOrDefault();
        if (user?.GetKey() != key)
        {
            throw new Exception("Invalid credentials");
        }

        return user;
    }

    public async Task<IUser> GetUserAsync(Guid id)
    {
        return await (await _users.FindAsync(u => u.Id == id)).FirstOrDefaultAsync();
    }
}
