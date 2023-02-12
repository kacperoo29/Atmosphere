using System.Net.Sockets;
using System.Net.WebSockets;
using Atmosphere.Application.Devices.Commands;
using Atmosphere.Application.Services;
using Atmosphere.Application.WebSockets;
using Atmosphere.Core.Models;
using MediatR;

namespace Atmosphere.Services;

public class DeviceHub : WebSocketHub<Device>
{
    public DeviceHub() : base() { }

    protected override async Task OnConnectedAsync(WebSocket socket, Guid? userId)
    {
        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }
    }

    protected override async Task OnMessageReceivedAsync(
        WebSocket socket,
        WebSocketReceiveResult result,
        ArraySegment<byte> buffer
    ) { }
}
