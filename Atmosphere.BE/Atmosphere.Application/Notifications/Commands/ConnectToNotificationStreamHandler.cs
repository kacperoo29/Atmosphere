using Atmosphere.Application.Services;
using Atmosphere.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Atmosphere.Application.Notfications.Commands;

public class ConnectToNotificationStreamHandler : IRequestHandler<ConnectToNotificationStream>
{
    private readonly IWebSocketHub<Notification> _notificationsHub;
    private readonly IHttpContextAccessor _httpContext;

    public ConnectToNotificationStreamHandler(
        IWebSocketHub<Notification> notificationsHub,
        IHttpContextAccessor httpContext
    )
    {
        _notificationsHub = notificationsHub;
        _httpContext = httpContext;
    }

    public async Task<Unit> Handle(
        ConnectToNotificationStream request,
        CancellationToken cancellationToken
    )
    {
        await _notificationsHub.ConnectAsync(
            this._httpContext.HttpContext.WebSockets.AcceptWebSocketAsync().Result
        );

        return Unit.Value;
    }
}
