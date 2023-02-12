using Atmosphere.Application.Services;
using Atmosphere.Core.Models;
using AutoMapper;

namespace Atmosphere.Application.DTO.Mapping;

public class DeviceConverter : ITypeConverter<Device, DeviceDto>
{
    private readonly WebSocketHub<Device> _deviceHub;

    public DeviceConverter(WebSocketHub<Device> deviceHub)
    {
        _deviceHub = deviceHub;
    }

    public DeviceDto Convert(Device source, DeviceDto destination, ResolutionContext context)
    {
        var connected = _deviceHub.ConnectedSockets.Any(x => x.UserId == source.Id);

        return new DeviceDto
        {
            Id = source.Id,
            Identifier = source.Identifier,
            IsActive = source.IsActive,
            IsConnected = connected,
        };
    }
}
