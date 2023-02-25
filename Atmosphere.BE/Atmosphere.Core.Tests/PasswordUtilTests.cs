using System.Text;
using Xunit;

namespace Atmosphere.Core.Tests;

public class PasswordUtilTests
{
    [Fact]
    public void PasswordIsHashed()
    {
        var password = "password";
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        var hashedPassword = PasswordUtil.GenerateSaltedHash(passwordBytes);
        
        Assert.NotEqual(passwordBytes, hashedPassword);
    }

    [Fact]
    public void PasswordIsVerified()
    {
        var password = "password";
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hashedPassword = PasswordUtil.GenerateSaltedHash(passwordBytes);

        var verified = PasswordUtil.VerifyPassword(password, hashedPassword);

        Assert.True(verified);
    }
}
