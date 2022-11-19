using Atmosphere.Application.Auth.Commands;
using Atmosphere.Application.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Atmosphere.Core.Enums;
using System.Net;
using Atmosphere.Application.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Authenticate([FromBody, BindRequired] Authenticate request)
    {
        try
        {
            var token = await _mediator.Send(request);

            return this.Ok(token);
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

}