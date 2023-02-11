using Atmosphere.Application.Services;
using Atmosphere.Core.Models;

namespace Atmosphere.Services;

public class DeviceService : IDeviceService
{
    private readonly IWebSocketHub<Device> _hub;
    private static readonly List<Guid> _devices = new();

    public List<Guid> ConnectedDevices => _devices;

    public DeviceService(IWebSocketHub<Device> hub)
    {
        _hub = hub;
    }

    public Task ConnectAsync(Guid userId)
    {
        if (_devices.Contains(userId))
        {
            return Task.CompletedTask;
        }

        _devices.Add(userId);

        return Task.CompletedTask;
    }

    public Task DisconnectAsync(Guid userId)
    {
        if (!_devices.Contains(userId))
        {
            return Task.CompletedTask;
        }

        _devices.Remove(userId);

        return Task.CompletedTask;
    }

    public async Task SendToAllAsync(string message)
    {
        if (_devices.Count == 0)
        {
            return;
        }

        await _hub.SendToAllAsync(message);
    }
}
