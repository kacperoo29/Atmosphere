namespace Atmosphere.Core.Models;

using System.Security.Claims;

public abstract class User : BaseModel
{
    public abstract List<Claim> GetClaims();
    public abstract string GetIdentifier();
    public abstract string GetKey();
}