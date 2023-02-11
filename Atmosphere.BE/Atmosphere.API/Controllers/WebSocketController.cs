using System.Net.WebSockets;
using System.Text;
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
        try
        {
            await _mediator.Send(new ConnectToNotificationStream());
        }
        catch (Exception e)
        {
            HttpContext.Response.StatusCode = 400;
            await HttpContext.Response.WriteAsync(e.Message);
        }
    }
}