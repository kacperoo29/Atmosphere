using System.Linq.Expressions;
using System.Text.Json;
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

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var entry = await _collection
            .Find(x => x.Key == key)
            .FirstOrDefaultAsync(cancellationToken);
        if (entry is null)
        {
            return default(T);
        }

        return (T?)entry.Value;
    }

    public async Task<IEnumerable<ConfigurationEntry>> GetEntiresAsync(
        Expression<Func<ConfigurationEntry, bool>>? predicate = null,
        CancellationToken cancellationToken = default
    )
    {
        return await _collection.Find(predicate ?? (x => true)).ToListAsync();
    }

    public async Task SetAsync(string key, object? value, CancellationToken cancellationToken = default)
    {
        if (value is JsonElement val)
        {
            switch (val.ValueKind)
            {
                case JsonValueKind.String:
                    value = val.GetString();
                    break;
                case JsonValueKind.Number:
                    value = val.GetDouble();
                    break;
                case JsonValueKind.True:
                    value = true;
                    break;
                case JsonValueKind.False:
                    value = false;
                    break;
                case JsonValueKind.Null:
                    value = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        var entry = ConfigurationEntry.Create(key, value);
        await _collection.ReplaceOneAsync(
            x => x.Key == key,
            entry,
            new ReplaceOptions { IsUpsert = true },
            cancellationToken
        );
    }
}
