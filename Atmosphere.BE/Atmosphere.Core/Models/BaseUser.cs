using System.Security.Claims;
using Atmoshpere.Core.Enums;
using Atmosphere.Core.Consts;

namespace Atmosphere.Core.Models;

public abstract class BaseUser : BaseModel
{
    public string Identifier { get; protected set; }
    public byte[] Password { get; protected set; }
    public bool IsActive { get; protected set; }
    public UserRole Role { get; protected set; }

    public virtual List<Claim> GetClaims()
    {
        return new List<Claim>
        {
            new(AtmosphereClaimTypes.UserId, Id.ToString()),
            new(ClaimTypes.Name, Identifier),
            new(ClaimTypes.Role, Role.ToString())
        };
    }
}