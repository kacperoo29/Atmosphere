using System.Net.WebSockets;
using System.Text;
using Atmosphere.Application.WebSockets;

namespace Atmosphere.Application.Services;

public abstract class WebSocketHub<T>
{
    private const int TIMEOUT = 1000 * 30;
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
        var wrapper = new WebSocketWrapper(socket, userId);
        Sockets.Add(wrapper);
        while (socket.State == WebSocketState.Open)
        {
            var start = DateTime.Now;
            var buffer = new ArraySegment<byte>(new byte[4096]);
            var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                await socket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    string.Empty,
                    CancellationToken.None
                );

                Sockets.Remove(wrapper);
            }
            else
            {
                // check if ping-pong
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                    if (message == "ping")
                    {
                        Console.WriteLine("ping");
                        try
                        {
                            await socket.SendAsync(
                                new ArraySegment<byte>(Encoding.UTF8.GetBytes("pong")),
                                WebSocketMessageType.Text,
                                true,
                                CancellationToken.None
                            );

                            wrapper.Age = 0;
                        }
                        catch (WebSocketException)
                        {
                            Sockets.Remove(wrapper);
                            try
                            {
                                await socket.CloseAsync(
                                    WebSocketCloseStatus.NormalClosure,
                                    string.Empty,
                                    CancellationToken.None
                                );
                            }
                            catch { }
                        }
                        continue;
                    }
                }

                await OnMessageReceivedAsync(socket, result, buffer);
            }

            wrapper.Age += (int)(DateTime.Now - start).TotalMilliseconds;
            if (wrapper.Age > TIMEOUT)
            {
                await socket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    string.Empty,
                    CancellationToken.None
                );

                Sockets.RemoveAll(s => s.UserId == userId);
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
