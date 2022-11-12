using MediatR;

namespace Atmosphere.Application.Auth.Commands;

public class Authenticate : IRequest<string>
{
    public string? Identifier { get; init; }
    public string? Key { get; init; }
}