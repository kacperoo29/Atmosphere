using Atmosphere.Application.DTO;
using Atmosphere.Core.Enums;
using Atmosphere.Core.Repositories;
using AutoMapper;
using MediatR;

namespace Atmosphere.Application.Devices.Queries;

public class GetAllDevicesHandler : IRequestHandler<GetAllDevices, List<DeviceDto>>
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IMapper _mapper;

    public GetAllDevicesHandler(IDeviceRepository deviceRepository, IMapper mapper)
    {
        _deviceRepository = deviceRepository;
        _mapper = mapper;
    }

    public async Task<List<DeviceDto>> Handle(GetAllDevices request, CancellationToken cancellationToken)
    {
        var devices = await _deviceRepository.GetAllAsync(x => x.Role == UserRole.Device);

        return _mapper.Map<List<DeviceDto>>(devices);
    }
}