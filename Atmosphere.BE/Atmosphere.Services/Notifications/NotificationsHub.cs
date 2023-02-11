using System.Net.WebSockets;
using Atmosphere.Application.Services;
using Atmosphere.Application.WebSockets;
using Atmosphere.Core.Models;
using MediatR;

namespace Atmosphere.Services.Notifications;

public class NotificationsHub : IWebSocketHub<Notification>
{
    private readonly IMediator _mediator;
    public List<WebSocketWrapper> Sockets { get; private set; } = new List<WebSocketWrapper>();

    public NotificationsHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task ConnectAsync(WebSocket socket, Guid? userId = null)
    {
        Sockets.Add(new WebSocketWrapper(socket, userId));

        while (socket.State == WebSocketState.Open)
        {
            var buffer = new ArraySegment<byte>(new byte[4096]);
            var result = await socket.ReceiveAsync(buffer, CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Close)
            {
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
