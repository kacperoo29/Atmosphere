using MediatR;

namespace Atmosphere.Application.Auth.Commands;

public class ActivateUser : IRequest
{
    public Guid Id { get; init; }
}