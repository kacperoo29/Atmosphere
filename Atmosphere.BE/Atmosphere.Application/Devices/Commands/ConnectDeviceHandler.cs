using Atmosphere.Application.Services;
using MediatR;

namespace Atmosphere.Application.Devices.Commands;

public class ConnectDeviceHandler : IRequestHandler<ConnectDevice>
{
    private readonly IDeviceService _deviceService;

    public ConnectDeviceHandler(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    public async Task<Unit> Handle(
        ConnectDevice request,
        CancellationToken cancellationToken
    )
    {
        await _deviceService.ConnectAsync(request.UserId);

        return Unit.Value;
    }
}
