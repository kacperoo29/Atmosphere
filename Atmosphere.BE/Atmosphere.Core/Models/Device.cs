using System.Security.Claims;
using System.Text;
using Atmosphere.Core.Enums;

namespace Atmosphere.Core.Models;

public class Device : BaseUser
{
    public string Identifier { get; protected set; }

    protected Device()
    {
        Username = string.Empty;
        Password = new byte[0];
        IsActive = false;
        Role = UserRole.Device;
        Identifier = string.Empty;
    }

    public static Device Create(string username, string identifier, string password)
    {
        var saltedPassword = PasswordUtil.GenerateSaltedHash(Encoding.UTF8.GetBytes(password));

        return new Device
        {
            Username = username,
            Password = saltedPassword,
            IsActive = false,
            Role = UserRole.Device,
            Identifier = identifier
        };
    }

    public override List<Claim> GetClaims()
    {
        var claims = base.GetClaims();

        return claims;
    }
}