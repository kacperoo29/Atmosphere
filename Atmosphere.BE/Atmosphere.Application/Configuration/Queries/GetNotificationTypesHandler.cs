namespace Atmosphere.Application.Configuration.Queries;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Enums;
using MediatR;

public class GetNotificationTypesHandler
    : IRequestHandler<GetNotificationTypes, List<NotificationType>>
{
    public GetNotificationTypesHandler() { }

    public async Task<List<NotificationType>> Handle(
        GetNotificationTypes request,
        CancellationToken cancellationToken
    )
    {
        return await Task.FromResult(
            Enum.GetValues<NotificationType>().Cast<NotificationType>().ToList()
        );
    }
}
