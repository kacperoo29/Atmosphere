namespace Atmosphere.Core.Repositories;

public interface IConfigurationRepository
{
    Task<object?> Get(string key);
    Task Set(string key, object? value);
}