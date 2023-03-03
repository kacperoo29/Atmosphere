using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Atmosphere.Application.Services;
using Atmosphere.Application.WebSockets;
using Atmosphere.Core.Models;

namespace Atmosphere.Services.Notifications;

public class WebSocketNotificationServiceDecorator : NotificationService
{
    private readonly WebSocketHub<Notification> _webSocketHub;

    public WebSocketNotificationServiceDecorator(
        INotificationService notificationService,
        WebSocketHub<Notification> webSocketHub
    ) : base(notificationService)
    {
        _webSocketHub = webSocketHub;
    }

    public override async Task Notify(Reading reading, IEnumerable<Notification> validationResults)
    {
        await _wrapee.Notify(reading, validationResults);

        if (WebSocketHub<Notification>.Sockets.Count == 0)
        {
            return;
        }

        if (validationResults.Any())
        {
            var message = new WebSocketPayload<IEnumerable<Notification>>()
            {
                Type = WebSocketPayloadType.Notification,
                Payload = validationResults
            };

            var json = JsonSerializer.Serialize(message, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            });

            await _webSocketHub.SendToAllAsync(json);
        }
    }
}
