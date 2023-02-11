using MediatR;

namespace Atmosphere.Application.Devices.Commands;

public class ConnectDevice : IRequest
{
    public Guid UserId { get; set; }
}