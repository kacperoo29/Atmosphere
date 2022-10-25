namespace Atmosphere.Core.Models;

using System.Security.Claims;
using System.Text;
using Atmoshpere.Core.Enums;

public class Device : BaseUser
{
    protected Device()
        : base()
    {
        Identifier = string.Empty;
        Password = new byte[0];
        IsActive = false;
        Role = UserRole.Device;
    }

    public static Device Create(string identifier, string password)
    {
        var saltedPassword = PasswordUtil.GenerateSaltedHash(Encoding.UTF8.GetBytes(password));

        return new Device
        {
            Identifier = identifier,
            Password = saltedPassword,
            IsActive = false,
            Role = UserRole.Device
        };
    }

    public override List<Claim> GetClaims()
    {
        var claims = base.GetClaims();

        return claims;
    }
}