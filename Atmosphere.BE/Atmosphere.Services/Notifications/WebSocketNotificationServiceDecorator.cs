using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
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
            var message = new StringBuilder();
            var type = WebSocketPayloadType.Notification;
            // build json response
            message.Append("{");
            message.Append($"\"type\": \"{type.ToString()}\",");
            message.Append("\"data\": [");
            foreach (var notification in validationResults)
            {
                message.Append($"\"{notification.Message}\",");
            }

            message.Remove(message.Length - 1, 1);
            message.Append("]}");

            await _webSocketHub.SendToAllAsync(message.ToString());
        }
    }
}
