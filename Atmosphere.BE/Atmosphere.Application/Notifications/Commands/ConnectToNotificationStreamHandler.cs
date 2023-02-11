using Atmosphere.Application.Services;
using Atmosphere.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Atmosphere.Application.Notfications.Commands;

public class ConnectToNotificationStreamHandler : IRequestHandler<ConnectToNotificationStream>
{
    private readonly IWebSocketHub<Notification> _notificationsHub;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IUserService _userService;

    public ConnectToNotificationStreamHandler(
        IWebSocketHub<Notification> notificationsHub,
        IHttpContextAccessor httpContext,
        IUserService userService
    )
    {
        _notificationsHub = notificationsHub;
        _httpContext = httpContext;
        _userService = userService;
    }

    public async Task<Unit> Handle(
        ConnectToNotificationStream request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userService.GetCurrentAsync();
        await _notificationsHub.ConnectAsync(
            await this._httpContext.HttpContext.WebSockets.AcceptWebSocketAsync(),
            user?.Id ?? null
        );

        return Unit.Value;
    }
}
