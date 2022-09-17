namespace Atmosphere.API.Controllers;

using Atmosphere.Application.Auth.Commands;

using MediatR;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Authenticate([FromBody] Authenticate request)
    {
        try
        {
            var token = await _mediator.Send(request);

            return Ok(token);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}