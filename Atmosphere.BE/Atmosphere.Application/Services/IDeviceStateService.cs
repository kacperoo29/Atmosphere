namespace Atmosphere.Application.Services;

public interface IDeviceStateService 
{
    Task<bool> IsDeviceOnline(Guid deviceId);
    Task SetDeviceOnline(Guid deviceId);
    Task SetDeviceOffline(Guid deviceId);
    Task SetLastSeen(Guid deviceId, DateTime lastSeen);
}