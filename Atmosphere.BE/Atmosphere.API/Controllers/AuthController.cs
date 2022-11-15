using Atmosphere.Application.Auth.Commands;
using Atmosphere.Application.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Atmosphere.Core.Enums;

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
            return this.Ok(await _mediator.Send(request));
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(nameof(UserRole.Admin))]
    public async Task<IActionResult> ActivateUser(Guid id)
    {
        try
        {
            return this.Ok(await _mediator.Send(new ActivateUser { Id = id }));
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

}