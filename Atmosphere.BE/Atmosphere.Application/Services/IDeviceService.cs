namespace Atmosphere.Application.Services;

public interface IDeviceService
{
    List<Guid> ConnectedDevices { get; }
    Task ConnectAsync(Guid userId);
    Task DisconnectAsync(Guid userId);
    Task SendToAllAsync(string message);
}