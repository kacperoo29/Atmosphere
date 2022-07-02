namespace Atmosphere.Core.Repositories;

using System.Linq.Expressions;

using Atmosphere.Core.Models;

public interface IConfigurationRepository
{
    Task<object?> Get(string key);
    Task Set(string key, object? value);
    Task<IEnumerable<ConfigurationEntry>> GetEntires(Expression<Func<ConfigurationEntry, bool>>? predicate = null);
}