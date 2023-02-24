using Atmosphere.Application.Auth.Commands;
using Atmosphere.Application.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Atmosphere.Core.Enums;
using System.Net;
using Atmosphere.Application.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Atmosphere.Application.Auth.Queries;

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
    [ProducesResponseType(typeof(AuthResponseDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Authenticate([FromBody, BindRequired] Authenticate request)
    {
        try
        {
            var token = await _mediator.Send(request);

            return this.Ok(token);
        }
        catch (UnauthorizedAccessException e)
        {
            return this.Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(DeviceDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> RegisterDevice([FromBody, BindRequired] RegisterDevice request)
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
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> ActivateUser([BindRequired] Guid id)
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

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(BaseUserDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            return this.Ok(await _mediator.Send(new GetCurrentUser()));
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPost]
    [Authorize(nameof(UserRole.Admin))]
    [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> CreateUser([FromBody, BindRequired] CreateUser request)
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

    [HttpGet]
    [Authorize(nameof(UserRole.Admin))]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            return this.Ok(await _mediator.Send(new GetUsers()));
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(nameof(UserRole.Admin))]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> RemoveUser([BindRequired] Guid id)
    {
        try
        {
            return this.Ok(await _mediator.Send(new RemoveUser { Id = id }));
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}