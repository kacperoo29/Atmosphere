namespace Atmosphere.Core.Models;

using System;
using System.Security.Claims;
using Atmosphere.Core.Consts;

public class Device : BaseModel, IUser
{
    public string Identifier { get; private set; }
    public string Key { get; private set; }
    public string Name { get; private set; }

    protected Device()
        : base()
    {
        Identifier = string.Empty;
        Name = string.Empty;
        Key = string.Empty;
    }

    public static Device Create(string identifier, string key, string name)
    {
        return new Device
        {
            Identifier = identifier,
            Key = key,
            Name = name
        };
    }

    public List<Claim> GetClaims()
    {
        return new List<Claim>
        {
            new Claim(AtmosphereClaimTypes.UserId, Id.ToString()),
            new Claim(ClaimTypes.SerialNumber, Identifier),
            new Claim(ClaimTypes.Name, Name)
        };
    }

    public string GetIdentifier()
    {
        return Identifier;
    }

    public string GetKey()
    {
        return Key;
    }
}