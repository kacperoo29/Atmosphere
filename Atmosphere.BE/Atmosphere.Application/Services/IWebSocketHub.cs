using System.Net.WebSockets;
using Atmosphere.Application.WebSockets;

namespace Atmosphere.Application.Services;

public interface IWebSocketHub<T>
{
    List<WebSocketWrapper> Sockets { get; }
    Task ConnectAsync(WebSocket socket, Guid? userId = null);
    Task SendToAllAsync(string message);
}
