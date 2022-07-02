namespace Atmosphere.Core.Models;

public class ConfigurationEntry : BaseModel
{
    public string Key { get; private set; }
    public object? Value { get; private set; }

    protected ConfigurationEntry()
        : base()
    {
        this.Key = string.Empty;
        this.Value = string.Empty;
    }

    public static ConfigurationEntry Create(string key, object? value)
    {
        return new ConfigurationEntry
        {
            Key = key,
            Value = value
        };
    }
}
