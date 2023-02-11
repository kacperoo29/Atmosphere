using System.Net;
using Atmosphere.Application.Devices.Queries;
using Atmosphere.Application.DTO;
using Atmosphere.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atmosphere.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DeviceController : ControllerBase
{
    private readonly IMediator _mediator;

    public DeviceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(typeof(List<DeviceDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var devices = await _mediator.Send(new GetAllDevices());

        return Ok(devices);
    }
}
