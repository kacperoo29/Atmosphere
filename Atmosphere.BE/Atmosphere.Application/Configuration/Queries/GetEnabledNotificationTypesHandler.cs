using Atmosphere.Application.Services;
using Atmosphere.Core.Enums;
using MediatR;

namespace Atmosphere.Application.Configuration.Queries;

public class GetEnabledNotificationTypesHandler : IRequestHandler<GetEnabledNotificationTypes, List<NotificationType>>
{
    private readonly IConfigService _configService;

    public GetEnabledNotificationTypesHandler(IConfigService configService)
    {
        _configService = configService;
    }

    public async Task<List<NotificationType>> Handle(GetEnabledNotificationTypes request, CancellationToken cancellationToken)
    {
        return await _configService.GetNotificationTypesAsync(cancellationToken);
    }
}