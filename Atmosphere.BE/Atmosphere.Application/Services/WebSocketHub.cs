using System.Net.WebSockets;
using System.Text;
using Atmosphere.Application.WebSockets;

namespace Atmosphere.Application.Services;

public abstract class WebSocketHub<T>
{
    private const int TIMEOUT = 1000 * 30;
    public static List<WebSocketWrapper> Sockets { get; protected set; } = new();

    protected WebSocketHub()
    {
    }

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
            new Thread(async () =>
            {
                Thread.CurrentThread.IsBackground = true;
                while (true)
                {
                    Thread.Sleep(1000);
                    wrapper.Age += 1000;
                    if (wrapper.Age > TIMEOUT)
                    {
                        try
                        {
                            await CloseSocket(socket);
                        }
                        catch { }
                        break;
                    }
                }
            }).Start();
        
            while (true)
            {
                var buffer = new ArraySegment<byte>(new byte[4096]);
                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await CloseSocket(socket);
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

                if (wrapper.Age > TIMEOUT)
                {
                    await CloseSocket(socket);
                    break;
                }
            }
        }
        finally
        {
            try
            {
                await CloseSocket(socket);
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
