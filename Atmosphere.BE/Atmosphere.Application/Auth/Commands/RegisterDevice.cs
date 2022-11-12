using Atmosphere.Core.Models;
using MediatR;

namespace Atmosphere.Application.Auth;

public class RegisterDevice : IRequest<Device>
{
    public string Identifier { get; init; }
    public string Password { get; init; }
}