using Atmosphere.Application.Services;
using Atmosphere.Core.Models;
using AutoMapper;

namespace Atmosphere.Application.DTO.Mapping;

public class DeviceConverter : ITypeConverter<Device, DeviceDto>
{
    private readonly IDeviceStateService _deviceStateService;

    public DeviceConverter(IDeviceStateService deviceStateService)
    {
        _deviceStateService = deviceStateService;
    }

    public DeviceDto Convert(Device source, DeviceDto destination, ResolutionContext context)
    {
        var connected = _deviceStateService.IsDeviceOnline(source.Id).Result;

        return new DeviceDto
        {
            Id = source.Id,
            Identifier = source.Identifier,
            IsActive = source.IsActive,
            IsConnected = connected,
        };
    }
}
