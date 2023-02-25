using System.Security.Claims;
using Atmosphere.Core.Enums;
using Atmosphere.Core.Models;
using Xunit;

namespace Atmosphere.Core.Tests;

public class UserTests
{
    [Fact]
    public void UserIsCreated()
    {
        var user = User.Create("username", "password");
        Assert.NotNull(user);
    }

    [Fact]
    public void UserHasProperRole()
    {
        var user = User.Create("username", "password");

        Assert.Equal(UserRole.User, user.Role);
    }

    [Fact]
    public void UserIsAdminAfterToggle()
    {
        var user = User.Create("username", "password");
        user.MakeAdmin();

        Assert.Equal(UserRole.Admin, user.Role);
    }

    [Fact]
    public void UserHasProperClaims()
    {
        var user = User.Create("username", "password");
        var claims = user.GetClaims();

        Assert.NotNull(claims);
        Assert.NotEmpty(claims);
        Assert.Contains(
            claims,
            c => c.Type == ClaimTypes.Role && c.Value == UserRole.User.ToString()
        );
    }

    [Fact]
    public void UserToggleActivate()
    {
        var user = User.Create("username", "password");
        var active = user.IsActive;

        user.ToggleActivate();

        Assert.NotEqual(active, user.IsActive);
    }
}
