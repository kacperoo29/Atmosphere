using Atmosphere.Application.Services;
using Atmosphere.Core.Models;
using AutoMapper;

namespace Atmosphere.Application.DTO.Mapping;

public class DeviceConverter : ITypeConverter<Device, DeviceDto>
{
    private readonly IDeviceService _deviceService;

    public DeviceConverter(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    public DeviceDto Convert(Device source, DeviceDto destination, ResolutionContext context)
    {
        var connected = _deviceService.ConnectedDevices.Contains(source.Id);

        return new DeviceDto
        {
            Id = source.Id,
            Identifier = source.Identifier,
            IsActive = source.IsActive,
            IsConnected = connected,
        };
    }
}
