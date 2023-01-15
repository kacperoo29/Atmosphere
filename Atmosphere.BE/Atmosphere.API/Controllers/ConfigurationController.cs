using System.Net;
using Atmosphere.Application.Configuration;
using Atmosphere.Application.Configuration.Commands;
using Atmosphere.Application.Configuration.Queries;
using Atmosphere.Application.DTO;
using Atmosphere.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Atmosphere.API.Controllers;

[Authorize(Roles = nameof(UserRole.Admin))]
[Route("api/[controller]/[action]")]
[ApiController]
public class ConfigurationController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConfigurationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> UpdateConfiguration(
        [FromBody, BindRequired] UpdateConfiguration request
    )
    {
        try
        {
            var configuration = await _mediator.Send(request);

            return Ok(configuration);
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> GetConfigurationEntry([FromQuery, BindRequired] string key)
    {
        try
        {
            var configuration = await _mediator.Send(new GetConfigurationEntry { Key = key });

            return Ok(configuration);
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(Dictionary<string, object?>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> GetConfigurationEntries(
        [FromQuery, BindRequired] string[] keys
    )
    {
        try
        {
            var configurations = await _mediator.Send(new GetConfigurationEntries { Keys = keys });

            return Ok(configurations);
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(Dictionary<string, object?>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> GetAllConfigurations()
    {
        try
        {
            var configurations = await _mediator.Send(new GetAllConfigurations());

            return Ok(configurations);
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPatch]
    [ProducesResponseType(typeof(EmailConfiguration), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> UpdateEmailConfiguration(
        [FromBody, BindRequired] UpdateEmailConfig request
    )
    {
        try
        {
            var configuration = await _mediator.Send(request);

            return Ok(configuration);
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(EmailConfiguration), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> GetEmailConfiguration()
    {
        try
        {
            var configuration = await _mediator.Send(new GetEmailConfiguration());

            return Ok(configuration);
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPut]
    [ProducesResponseType(typeof(IEnumerable<NotificationType>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> ToggleNotificationType(
        [FromBody, BindRequired] ToggleNotificationType request
    )
    {
        try
        {
            var configuration = await _mediator.Send(request);

            return Ok(configuration);
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

}
