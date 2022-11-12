using System.Linq.Expressions;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using MongoDB.Driver;

namespace Atmosphere.Services.Repositories;

public class ConfigurationRepository : IConfigurationRepository
{
    private readonly IMongoCollection<ConfigurationEntry> _collection;

    public ConfigurationRepository(IMongoCollection<ConfigurationEntry> collection)
    {
        _collection = collection;
    }

    public async Task<object?> Get(string key)
    {
        var entry = await _collection.Find(x => x.Key == key).FirstOrDefaultAsync();

        return entry?.Value;
    }

    public async Task<IEnumerable<ConfigurationEntry>> GetEntires(
        Expression<Func<ConfigurationEntry, bool>>? predicate = null)
    {
        return await _collection.Find(predicate ?? (x => true)).ToListAsync();
    }

    public async Task Set(string key, object? value)
    {
        var entry = ConfigurationEntry.Create(key, value);

        await _collection.ReplaceOneAsync(x => x.Key == key, entry, new ReplaceOptions { IsUpsert = true });
    }
}