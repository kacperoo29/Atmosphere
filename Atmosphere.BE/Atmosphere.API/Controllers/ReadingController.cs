namespace Atmosphere.API.Controllers;

using Atmosphere.Application;
using Atmosphere.Application.Readings.Commands;
using Atmosphere.Application.Readings.Queries;
using Atmosphere.Core.Models;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]/[action]")]
[ApiController]
public class ReadingController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReadingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateReading([FromBody] CreateReading request)
    {
        try
        {
            var reading = await _mediator.Send(request);

            return Ok(reading);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllReadings([FromQuery] Guid deviceId)
    {
        try
        {
            var readings = await _mediator.Send(new GetAllReadings { DeviceId = deviceId });

            return Ok(readings);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}