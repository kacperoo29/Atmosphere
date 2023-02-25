using Atmosphere.Core.Models;
using Xunit;

namespace Atmosphere.Core.Tests;

public class ConfigurationEntryTests
{
    [Fact]
    public void ConfigurationEntryIsCreated()
    {
        var key = "key";
        var value = "value";

        var entry = ConfigurationEntry.Create(key,value);

        Assert.NotNull(entry);
        Assert.Equal(key, entry.Key);
        Assert.Equal(value, entry.Value);
    }
}