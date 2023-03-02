using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Atmosphere.Application.Devices.Commands;
using Atmosphere.Application.Readings.Commands;
using Atmosphere.Application.Services;
using Atmosphere.Application.WebSockets;
using Atmosphere.Core.Enums;
using Atmosphere.Core.Models;
using MediatR;

namespace Atmosphere.Services;

public class DeviceHub : WebSocketHub<Device>
{
    private readonly IMediator _mediator;

    public DeviceHub(IMediator mediator) : base()
    {
        _mediator = mediator;
    }

    protected override async Task OnConnectedAsync(WebSocket socket, Guid? userId)
    {
        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }
    }

    protected override async Task OnMessageReceivedAsync(
        WebSocket socket,
        WebSocketReceiveResult result,
        ArraySegment<byte> buffer
    )
    {
        var message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
        var msg = JsonSerializer.Deserialize<Msg>(
            message,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            }
        );
        if (msg?.Type == "reading")
        {
            ResetAge(socket);
            var reading = msg.Payload;
            if (reading == null)
            {
                return;
            }

            await _mediator.Send(reading.ToCreateReading());
        }
    }

    private class Msg
    {
        public string Type { get; set; }
        public ReadingInternal Payload { get; set; }
    }

    private class ReadingInternal
    {
        public decimal Value { get; set; }
        public string Unit { get; set; }
        public ReadingType Type { get; set; }
        public string DeviceAddress { get; set; }
        public DateTime Timestamp { get; set; }

        public CreateReading ToCreateReading()
        {
            return new()
            {
                Value = this.Value,
                Unit = this.Unit,
                Type = this.Type,
                Timestamp = this.Timestamp
            };
        }
    }
}
