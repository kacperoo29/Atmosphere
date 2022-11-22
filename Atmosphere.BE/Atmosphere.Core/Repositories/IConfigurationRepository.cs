using System.Linq.Expressions;
using Atmosphere.Core.Models;

namespace Atmosphere.Core.Repositories;

public interface IConfigurationRepository
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
    Task SetAsync(string key, object? value, CancellationToken cancellationToken = default);
    Task<IEnumerable<ConfigurationEntry>> GetEntiresAsync(
        Expression<Func<ConfigurationEntry, bool>>? predicate = null,
        CancellationToken cancellationToken = default
    );
}
