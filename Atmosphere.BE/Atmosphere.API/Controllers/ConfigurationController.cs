namespace Atmosphere.API.Controllers;

using Atmosphere.Application.Configuration.Commands;
using Atmosphere.Application.Configuration.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> UpdateConfiguration([FromBody] UpdateConfiguration request)
    {
        try
        {
            var configuration = await _mediator.Send(request);

            return Ok(configuration);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetConfigurationEntry([FromQuery] string key)
    {
        try
        {
            var configuration = await _mediator.Send(new GetConfigurationEntry { Key = key });

            return Ok(configuration);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetConfigurationEntries([FromQuery] string[] keys)
    {
        try
        {
            var configurations = await _mediator.Send(new GetConfigurationEntries { Keys = keys });

            return Ok(configurations);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllConfigurations()
    {
        try
        {
            var configurations = await _mediator.Send(new GetAllConfigurations());

            return Ok(configurations);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}