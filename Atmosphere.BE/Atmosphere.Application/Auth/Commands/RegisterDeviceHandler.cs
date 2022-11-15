using Atmosphere.Application.Services;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using MediatR;

namespace Atmosphere.Application.Auth.Commands;

public class RegisterDeviceHandler : IRequestHandler<RegisterDevice, Device>
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IUserService _userService;

    public RegisterDeviceHandler(IUserService userRepository, IDeviceRepository deviceRepository)
    {
        _userService = userRepository;
        _deviceRepository = deviceRepository;
    }

    public async Task<Device> Handle(RegisterDevice request, CancellationToken cancellationToken)
    {
        var device = Device.Create(request.Identifier, request.Password);

        return await _userService.CreateUserAsync(device) as Device;
    }
}