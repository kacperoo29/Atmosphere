using System.Net.WebSockets;
using Atmosphere.Application.Services;
using Atmosphere.Application.WebSockets;
using Atmosphere.Core.Models;
using MediatR;

namespace Atmosphere.Services.Notifications;

public class NotificationsHub : WebSocketHub<Notification>
{
    public NotificationsHub() : base() { }

    protected override async Task OnConnectedAsync(WebSocket socket, Guid? userId) { }

    protected override async Task OnDisconnectedAsync(WebSocket socket) { }

    protected override async Task OnMessageReceivedAsync(
        WebSocket socket,
        WebSocketReceiveResult result,
        ArraySegment<byte> buffer
    ) { }
}
