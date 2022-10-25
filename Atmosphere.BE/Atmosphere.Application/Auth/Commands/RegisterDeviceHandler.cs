using Atmoshpere.Application.Services;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;

using MediatR;

namespace Atmosphere.Application.Auth.Commands;

public class RegisterDeviceHandler : IRequestHandler<RegisterDevice, Device>
{
    private readonly IUserService _userService;
    private readonly IDeviceRepository _deviceRepository;

    public RegisterDeviceHandler(IUserService userRepository, IDeviceRepository deviceRepository)
    {
        _userService = userRepository;
        _deviceRepository = deviceRepository;
    }

    public async Task<Device> Handle(RegisterDevice request, CancellationToken cancellationToken)
    {
        var device = Device.Create(request.Identifier, request.Password);
        
        return await _userService.CreateUser(device) as Device;
    }
}