using Atmosphere.Application.Auth;
using Atmosphere.Application.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Atmosphere.API.Controllers;

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

    [HttpPost]
    public async Task<IActionResult> RegisterDevice([FromBody] RegisterDevice request)
    {
        try
        {
            return Ok(await _mediator.Send(request));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}