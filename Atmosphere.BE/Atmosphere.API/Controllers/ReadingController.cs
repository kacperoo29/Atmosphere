using Atmosphere.Application.Readings.Commands;
using Atmosphere.Application.Readings.Queries;
using Atmosphere.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atmosphere.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ReadingController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReadingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(nameof(UserRole.Device))]
    public async Task<IActionResult> CreateReading([FromBody] CreateReading request)
    {
        try
        {
            var reading = await _mediator.Send(request);

            return this.Ok(reading);
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

    [HttpGet]
    public async Task<IActionResult> GetAllReadings([FromQuery] Guid deviceId)
    {
        try
        {
            var readings = await _mediator.Send(new GetAllReadings { DeviceId = deviceId });

            return this.Ok(readings);
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}