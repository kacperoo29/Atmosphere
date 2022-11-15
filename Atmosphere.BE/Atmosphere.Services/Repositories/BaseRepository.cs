using System.Linq.Expressions;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using MongoDB.Driver;

namespace Atmosphere.Services.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
{
    protected readonly IMongoCollection<T> _collection;

    public BaseRepository(IMongoCollection<T> collection)
    {
        _collection = collection;
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity, cancellationToken: cancellationToken);
    }

    public async Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _collection.DeleteOneAsync(x => x.Id == entity.Id, cancellationToken);
    }

    public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,
        CancellationToken cancellationToken = default)
    {
        return await _collection.Find(filter ?? (x => true)).ToListAsync();
    }
}