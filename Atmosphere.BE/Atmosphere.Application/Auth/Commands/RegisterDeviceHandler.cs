using Atmosphere.Application.DTO;
using Atmosphere.Application.Services;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using AutoMapper;
using MediatR;

namespace Atmosphere.Application.Auth.Commands;

public class RegisterDeviceHandler : IRequestHandler<RegisterDevice, DeviceDto>
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public RegisterDeviceHandler(IUserService userRepository, IDeviceRepository deviceRepository, IMapper mapper)
    {
        _userService = userRepository;
        _deviceRepository = deviceRepository;
        _mapper = mapper;
    }

    public async Task<DeviceDto> Handle(RegisterDevice request, CancellationToken cancellationToken)
    {
        var device = Device.Create(request.Username, request.Identifier, request.Password);
        device = await _userService.CreateUserAsync(device) as Device ?? throw new Exception("Failed to create device");

        return this._mapper.Map<DeviceDto>(device);
    }
}