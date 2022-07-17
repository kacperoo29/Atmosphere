namespace Atmosphere.Core.Models;

using System;
using System.Security.Claims;

public class Device : BaseModel, IUser
{
    public string Identifier { get; private set; }
    public string Name { get; private set; }

    protected Device()
        : base()
    {
        Identifier = string.Empty;
        Name = string.Empty;
    }

    public static Device Create(string identifier, string name)
    {
        return new Device
        {
            Identifier = identifier,
            Name = name
        };
    }

    public List<Claim> GetClaims()
    {
        return new List<Claim>
        {
            new Claim(ClaimTypes.SerialNumber, Identifier),
            new Claim(ClaimTypes.Name, Name)
        };
    }
}