using System.Net;
using Aqua.EnumerableExtensions;
using Atmosphere.Application.DTO;
using Atmosphere.Application.Readings.Commands;
using Atmosphere.Application.Readings.Queries;
using Atmosphere.Core;
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

    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.Device))]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateReadings(
        [FromBody, BindRequired] List<CreateReading> requests
    )
    {
        try
        {
            foreach (var request in requests)
            {
                await _mediator.Send(request);
            }

            return this.Ok();
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(PagedList<ReadingDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPagedReadings(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50
    )
    {
        try
        {
            var readings = await _mediator.Send(
                new GetAllPagedReadings { PageNumber = pageNumber, PageSize = pageSize }
            );

            return this.Ok(readings);
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(PagedList<ReadingDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPagedReadingsByDevice(
        [FromQuery] Guid deviceId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null
    )
    {
        try
        {
            var readings = await _mediator.Send(
                new GetPagedReadingsByDevice
                {
                    DeviceId = deviceId,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    StartDate = startDate,
                    EndDate = endDate
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
    [Authorize]
    [ProducesResponseType(typeof(PagedList<ReadingDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPagedReadingsByDate(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50
    )
    {
        try
        {
            var readings = await _mediator.Send(
                new GetPagedReadingsByDate
                {
                    StartDate = startDate ?? DateTime.MinValue,
                    EndDate = endDate ?? DateTime.MaxValue,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                }
            );

            return this.Ok(readings);
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(Dictionary<DateTime, decimal>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetChartData(
        [FromBody, BindRequired] GetChartData request
    )
    {
        try
        {
            var readings = await _mediator.Send(request);

            return this.Ok(readings);
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}
