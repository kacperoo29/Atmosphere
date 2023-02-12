using Atmosphere.Application.Services;
using Atmosphere.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Atmosphere.Application.Devices.Commands;

public class ConnectToDeviceStreamHandler : IRequestHandler<ConnectToDeviceStream>
{
    private readonly WebSocketHub<Device> _deviceService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;

    public ConnectToDeviceStreamHandler(
        WebSocketHub<Device> deviceService,
        IHttpContextAccessor httpContextAccessor,
        IUserService userService
    )
    {
        _deviceService = deviceService;
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
    }

    public async Task<Unit> Handle(
        ConnectToDeviceStream request,
        CancellationToken cancellationToken
    )
    {
        var device = await _userService.GetCurrentAsync();
        if (device == null)
        {
            throw new UnauthorizedAccessException();
        }

        await _deviceService.ConnectAsync(
            await this._httpContextAccessor.HttpContext.WebSockets.AcceptWebSocketAsync(),
            device.Id
        );

        return Unit.Value;
    }
}
