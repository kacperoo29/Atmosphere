namespace Atmosphere.Core.Repositories;

public interface IConfigurationRepository
{
    Task<string> Get(string key);
    Task Set(string key, string value);
}