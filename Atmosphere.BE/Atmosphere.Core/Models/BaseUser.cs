using System.Security.Claims;
using Atmosphere.Core.Enums;
using Atmosphere.Core.Consts;

namespace Atmosphere.Core.Models;

public abstract class BaseUser : BaseModel
{
    public string Username { get; protected set; }
    public byte[] Password { get; protected set; }
    public bool IsActive { get; protected set; }
    public UserRole Role { get; protected set; }

    public virtual List<Claim> GetClaims()
    {
        return new List<Claim>
        {
            new(AtmosphereClaimTypes.UserId, Id.ToString()),
            new(ClaimTypes.Name, Username),
            new(ClaimTypes.Role, Role.ToString())
        };
    }

    public void ToggleActivate()
    {
        IsActive = !IsActive;
        Update();
    }
}