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
        await _configuration.SendToAllAsync(JsonSerializer.Serialize(new
        {
            type = "pollingRate",
            payload = request.PollingRate
        }));

        return Unit.Value;
    }
}