using System.Net.WebSockets;
using Atmosphere.Application.Devices.Commands;
using Atmosphere.Application.Services;
using Atmosphere.Application.WebSockets;
using Atmosphere.Core.Models;
using MediatR;

namespace Atmosphere.Services;

public class DeviceHub : IWebSocketHub<Device>
{
    private readonly IMediator _mediator;

    public DeviceHub(IMediator mediator)
    {
        Sockets = new List<WebSocketWrapper>();
        _mediator = mediator;
    }

    public List<WebSocketWrapper> Sockets { get; private set; }

    public async Task ConnectAsync(WebSocket socket, Guid? userId = null)
    {
        if (userId == null)
        {
            return;
        }

        Sockets.Add(new WebSocketWrapper(socket, userId));
        await _mediator.Send(new ConnectDevice { UserId = userId.Value });

        while (socket.State == WebSocketState.Open)
        {
            var buffer = new ArraySegment<byte>(new byte[4096]);
            var result = await socket.ReceiveAsync(buffer, CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                await _mediator.Send(new DisconnectDevice { UserId = userId.Value });

                await socket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    string.Empty,
                    CancellationToken.None
                );
            }
        }
    }

    public async Task SendToAllAsync(string message)
    {
        if (Sockets.Count == 0)
        {
            return;
        }

        foreach (var socket in Sockets)
        {
            if (socket.Socket.State != WebSocketState.Open)
            {
                continue;
            }

            await socket.SendAsync(message);
        }
    }
}
