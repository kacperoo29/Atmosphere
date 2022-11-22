using Atmosphere.Application.DTO;
using MediatR;

namespace Atmosphere.Application.Configuration.Queries;

public class GetNotificationSettings : IRequest<NotificationSettingsDto>
{
}