using Atmosphere.Application.Services;
using Atmosphere.Core.Enums;
using Atmosphere.Core.Models;
using MediatR;

namespace Atmosphere.Application.Configuration.Commands;

public class ToggleNotificationTypeHandler
    : IRequestHandler<ToggleNotificationType, IEnumerable<NotificationType>>
{
    private readonly IConfigService _configService;

    public ToggleNotificationTypeHandler(IConfigService configService)
    {
        _configService = configService;
    }

    public async Task<IEnumerable<NotificationType>> Handle(
        ToggleNotificationType request,
        CancellationToken cancellationToken
    )
    {
        var types = await _configService.GetNotificationTypesAsync();
        if (types.Contains(request.Type))
        {
            types.Remove(request.Type);
        }
        else
        {
            types.Add(request.Type);
        }

        await _configService.UpdateNotificationTypesAsync(types, cancellationToken);

        return types;
    }
}
