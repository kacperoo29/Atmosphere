namespace Atmosphere.Core.Models;

using System.Security.Claims;
using Atmoshpere.Core.Enums;
using Atmosphere.Core.Consts;

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
            new Claim(AtmosphereClaimTypes.UserId, this.Id.ToString()),
            new Claim(ClaimTypes.Name, this.Identifier),
            new Claim(ClaimTypes.Role, this.Role.ToString())
        };
    }
}