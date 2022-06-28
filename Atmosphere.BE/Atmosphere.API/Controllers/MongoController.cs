namespace Atmosphere.API.Controllers;

using Atmosphere.Application.Readings.Commands;
using Atmosphere.Application.Readings.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]/[action]")]
[ApiController]
public class MongoController : ControllerBase
{
    private readonly IMediator _mediator;

    public MongoController(IMediator mediator)
    {
        _mediator = mediator;
    }

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
            var readings = await _mediator.Send(new GetAllReadings {DeviceId = deviceId});

            return Ok(readings);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}