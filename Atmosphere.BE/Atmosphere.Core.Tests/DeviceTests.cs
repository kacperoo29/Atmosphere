using System.Security.Claims;
using Atmosphere.Core.Enums;
using Atmosphere.Core.Models;
using Xunit;

namespace Atmosphere.Core.Tests;

public class DeviceTests
{
    [Fact]
    public void DeviceIsCreated()
    {
        var device = Device.Create("username", "identifier", "password");
        Assert.NotNull(device);
    }

    [Fact]
    public void DeviceHasProperRoleAfterCreate()
    {
        var device = Device.Create("username", "identifier", "password");
        Assert.Equal(UserRole.Device, device.Role);
    }

    [Fact]
    public void DeviceHasProperClaims()
    {
        var device = Device.Create("username", "identifier", "password");
        var claims = device.GetClaims();
        Assert.NotNull(claims);
        Assert.NotEmpty(claims);
        Assert.Contains(
            claims,
            c => c.Type == ClaimTypes.Role && c.Value == UserRole.Device.ToString()
        );
    }
}
