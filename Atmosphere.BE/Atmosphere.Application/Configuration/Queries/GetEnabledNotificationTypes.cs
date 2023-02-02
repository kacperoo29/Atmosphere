using Atmosphere.Core.Enums;
using MediatR;

namespace Atmosphere.Application.Configuration.Queries;

public class GetEnabledNotificationTypes : IRequest<List<NotificationType>>
{
}