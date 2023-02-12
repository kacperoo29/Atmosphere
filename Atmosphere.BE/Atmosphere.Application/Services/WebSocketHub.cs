using System.Net.WebSockets;
using Atmosphere.Application.WebSockets;

namespace Atmosphere.Application.Services;

public abstract class WebSocketHub<T>
{
    public List<WebSocketWrapper> Sockets { get; protected set; }

    protected WebSocketHub()
    {
        Sockets = new();
    }

    public List<WebSocketWrapper> ConnectedSockets =>
        Sockets.FindAll(s => s.Socket.State == WebSocketState.Open);

    public async Task ConnectAsync(WebSocket socket, Guid? userId = null)
    {
        await OnConnectedAsync(socket, userId);
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

                Sockets.RemoveAll(s => s.UserId == userId);
            }
            else
            {
                await OnMessageReceivedAsync(socket, result, buffer);
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

    protected abstract Task OnMessageReceivedAsync(
        WebSocket socket,
        WebSocketReceiveResult result,
        ArraySegment<byte> buffer
    );
    protected abstract Task OnConnectedAsync(WebSocket socket, Guid? userId = null);
}
