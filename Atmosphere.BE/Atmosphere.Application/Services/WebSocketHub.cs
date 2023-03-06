using System.Net.WebSockets;
using System.Text;
using Atmosphere.Application.WebSockets;

namespace Atmosphere.Application.Services;

public abstract class WebSocketHub<T>
{
    private const int TIMEOUT = 1000 * 30;
    public static List<WebSocketWrapper> Sockets { get; protected set; }

    protected WebSocketHub()
    {
        Sockets = new();
    }

    public List<WebSocketWrapper> ConnectedSockets =>
        Sockets.FindAll(s => s.Socket.State == WebSocketState.Open);

    public async Task ConnectAsync(WebSocket socket, Guid? userId = null)
    {
        await OnConnectedAsync(socket, userId);
        var wrapper = new WebSocketWrapper(socket, userId);
        if (Sockets.Find(s => s.UserId == userId) != null)
        {
            Sockets.Remove(Sockets.Find(s => s.UserId == userId));
        }
        
        Sockets.Add(wrapper);
        try
        {
            while (true)
            {
                var start = DateTime.Now;
                var buffer = new ArraySegment<byte>(new byte[4096]);
                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await CloseSocket(socket);
                    Sockets.Remove(wrapper);
                    break;
                }
                else
                {
                    // check if ping-pong
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                        if (message == "ping")
                        {
                            await socket.SendAsync(
                                new ArraySegment<byte>(Encoding.UTF8.GetBytes("pong")),
                                WebSocketMessageType.Text,
                                true,
                                CancellationToken.None
                            );

                            wrapper.Age = 0;
                            continue;
                        }
                    }

                    await OnMessageReceivedAsync(socket, result, buffer);
                }

                wrapper.Age += (int)(DateTime.Now - start).TotalMilliseconds;
                if (wrapper.Age > TIMEOUT)
                {
                    await CloseSocket(socket);
                    Sockets.Remove(wrapper);
                    break;
                }
            }
        }
        finally
        {
            try
            {
                await CloseSocket(socket);
                Sockets.Remove(wrapper);
            }
            catch { }
        }
    }

    public async Task SendToAllAsync(string message)
    {
        foreach (var socket in Sockets)
        {
            try
            {
                await socket.SendAsync(message);
            }
            catch { }
        }
    }

    private async Task CloseSocket(WebSocket socket)
    {
        try
        {
            Console.WriteLine("Closing socket");
            await OnDisconnectedAsync(socket);
            await socket.CloseAsync(
                WebSocketCloseStatus.NormalClosure,
                string.Empty,
                CancellationToken.None
            );
        }
        catch { }
    }

    protected void ResetAge(WebSocket socket)
    {
        var wrapper = Sockets.Find(s => s.Socket == socket);
        if (wrapper != null)
        {
            wrapper.Age = 0;
        }
    }

    protected abstract Task OnMessageReceivedAsync(
        WebSocket socket,
        WebSocketReceiveResult result,
        ArraySegment<byte> buffer
    );
    protected abstract Task OnConnectedAsync(WebSocket socket, Guid? userId = null);
    protected abstract Task OnDisconnectedAsync(WebSocket socket);
}
