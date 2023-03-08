using System.Text.Json;
using Atmosphere.Application.Services;
using Atmosphere.Core.Models;
using MediatR;

namespace Atmosphere.Application.Configuration.Commands;

public class SetPollingRateHandler : IRequestHandler<SetPollingRate>
{
    private readonly WebSocketHub<Device> _configuration;

    public SetPollingRateHandler(WebSocketHub<Device> configuration)
    {
        _configuration = configuration;
    }

    public async Task<Unit> Handle(SetPollingRate request, CancellationToken cancellationToken)
    {
        var message = JsonSerializer.Serialize(new
        {
            type = "pollingRate",
            payload = request.PollingRate
        });

        if (request.DeviceId is null)
        {
            await _configuration.SendToAllAsync(message);
        }
        else
        {
            await _configuration.SendToAsync(request.DeviceId.Value, message);
        }

        return Unit.Value;
    }
}