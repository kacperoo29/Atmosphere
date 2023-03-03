namespace Atmosphere.Application.WebSockets;

public class WebSocketPayload<T>
{
    public WebSocketPayloadType Type { get; set; }
    public T Payload { get; set; }
}
