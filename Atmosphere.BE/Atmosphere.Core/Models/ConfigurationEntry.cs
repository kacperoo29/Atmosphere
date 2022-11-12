namespace Atmosphere.Core.Models;

public class ConfigurationEntry : BaseModel
{
    protected ConfigurationEntry()
    {
        Key = string.Empty;
        Value = string.Empty;
    }

    public string Key { get; private set; }
    public object? Value { get; private set; }

    public static ConfigurationEntry Create(string key, object? value)
    {
        return new ConfigurationEntry
        {
            Key = key,
            Value = value
        };
    }
}