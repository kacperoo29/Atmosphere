using Atmosphere.Application.Services;

namespace Atmosphere.Services;

public class DeviceStateService : IDeviceStateService
{
    private static readonly TimeSpan DeviceOnlineTimeout = TimeSpan.FromSeconds(30);
    private static Dictionary<Guid, DateTime> _deviceStates = new();

    public async Task<bool> IsDeviceOnline(Guid deviceId)
    {
        if (_deviceStates.TryGetValue(deviceId, out var lastSeen))
        {
            return DateTime.UtcNow - lastSeen < DeviceOnlineTimeout;
        }

        return false;
    }

    public async Task SetDeviceOffline(Guid deviceId)
    {
        _deviceStates.Remove(deviceId);
    }

    public async Task SetDeviceOnline(Guid deviceId)
    {
        _deviceStates[deviceId] = DateTime.UtcNow;
    }

    public async Task SetLastSeen(Guid deviceId, DateTime lastSeen)
    {
        _deviceStates[deviceId] = lastSeen;
    }
}