namespace Atmosphere.Core.Models;

using System.Security.Claims;

public interface IUser
{
    Guid Id { get; }
    List<Claim> GetClaims();
}