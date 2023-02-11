using System.Net;
using Atmosphere.Application.DTO;
using Atmosphere.Application.Readings.Commands;
using Atmosphere.Application.Readings.Queries;
using Atmosphere.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
    [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.Device))]
    [ProducesResponseType(typeof(ReadingDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> CreateReading([FromBody, BindRequired] CreateReading request)
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
    [ProducesResponseType(typeof(List<ReadingDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllReadings()
    {
        try
        {
            var readings = await _mediator.Send(new GetAllReadings { });

            return this.Ok(readings);
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ReadingDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetReadingsByDate(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null
    )
    {
        try
        {
            var readings = await _mediator.Send(
                new GetReadingsByDate
                {
                    StartDate = startDate ?? DateTime.MinValue,
                    EndDate = endDate ?? DateTime.MaxValue
                }
            );

            return this.Ok(readings);
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ReadingDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetReadingsByDevice([FromQuery] Guid deviceId)
    {
        try
        {
            var readings = await _mediator.Send(new GetReadingsByDevice { DeviceId = deviceId });

            return this.Ok(readings);
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}
