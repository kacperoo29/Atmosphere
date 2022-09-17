namespace Atmosphere.Application.Auth.Commands;

using MediatR;

public class Authenticate : IRequest<string>
{
    public string? Identifier { get; init; }
    public string? Key { get; init; }
}