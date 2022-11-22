using Atmosphere.Application.Services;
using MediatR;

namespace Atmosphere.Application.Configuration.Commands;

public class UpdateNotificationSettingsHandler : IRequestHandler<UpdateNotificationSettings>
{
    private readonly IConfigService _settingsService;

    public UpdateNotificationSettingsHandler(IConfigService settingsService)
    {
        _settingsService = settingsService;
    }

    public async Task<Unit> Handle(UpdateNotificationSettings request, CancellationToken cancellationToken)
    {
        var settings = await _settingsService.GetNotificationSettingsAsync(cancellationToken);
        settings.EmailEnabled = request.EmailEnabled;
        settings.EmailTo = request.EmailTo;
        settings.TemperatureThresholdMin = request.TemperatureThresholdMin;
        settings.TemperatureThresholdMax = request.TemperatureThresholdMax;

        await _settingsService.UpdateNotificationSettingsAsync(settings, cancellationToken);

        return Unit.Value;
    }
}