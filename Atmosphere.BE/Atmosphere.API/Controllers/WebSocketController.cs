using System.Net.WebSockets;
using System.Text;
using Atmosphere.Application.Devices.Commands;
using Atmosphere.Application.Notfications;
using Atmosphere.Application.Notfications.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atmosphere.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class WebSocketController : ControllerBase
{
    private readonly IMediator _mediator;

    public WebSocketController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task Notifications()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            await _mediator.Send(new ConnectToNotificationStream());
        }
    }

    [HttpGet]
    [Authorize]
    public async Task Devices()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            await _mediator.Send(new ConnectToDeviceStream());
        }
    }
}
