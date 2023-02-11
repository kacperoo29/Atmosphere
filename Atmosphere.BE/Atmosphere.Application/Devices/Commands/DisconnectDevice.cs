using MediatR;

namespace Atmosphere.Application.Devices.Commands;

public class DisconnectDevice : IRequest
{
    public Guid UserId { get; set; }
}
