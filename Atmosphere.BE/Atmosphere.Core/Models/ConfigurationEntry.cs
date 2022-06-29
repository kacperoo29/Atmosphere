namespace Atmosphere.Core.Models;

public class ConfigurationEntry : BaseModel
{
    public string Key { get; private set; }
    public string Value { get; private set; }

    protected ConfigurationEntry()
        : base()
    {
        this.Key = string.Empty;
        this.Value = string.Empty;
    }

    public static ConfigurationEntry Create(string key, string value)
    {
        return new ConfigurationEntry
        {
            Key = key,
            Value = value
        };
    }
}
