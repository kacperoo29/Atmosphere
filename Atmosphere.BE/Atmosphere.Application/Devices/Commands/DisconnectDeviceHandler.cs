using Atmosphere.Application.Services;
using MediatR;

namespace Atmosphere.Application.Devices.Commands;

public class DisconnectDeviceHandler : IRequestHandler<DisconnectDevice>
{
    private readonly IDeviceService _deviceService;

    public DisconnectDeviceHandler(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    public async Task<Unit> Handle(
        DisconnectDevice request,
        CancellationToken cancellationToken
    )
    {
        await _deviceService.DisconnectAsync(request.UserId);

        return Unit.Value;
    }
}
