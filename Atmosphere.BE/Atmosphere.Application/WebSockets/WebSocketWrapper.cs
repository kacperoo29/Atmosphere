using System.Net.WebSockets;
using System.Text;

namespace Atmosphere.Application.WebSockets;

public class WebSocketWrapper
{
    public Guid? UserId { get; private set; }
    public WebSocket Socket { get; private set; }
    public int Age { get; set; }

    public WebSocketWrapper(WebSocket socket, Guid? userId = null)
    {
        Socket = socket;
        UserId = userId;
        Age = 0;
    }

    public async Task SendAsync(string message)
    {
        Console.WriteLine($"Sending message to {UserId}: {message}");
        var encoded = Encoding.UTF8.GetBytes(message);
        var buffer = new ArraySegment<byte>(encoded, 0, encoded.Length);

        await Socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
    }
}
