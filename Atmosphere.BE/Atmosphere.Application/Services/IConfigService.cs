using Atmosphere.Application.Configuration;
using Atmosphere.Core.Enums;

namespace Atmosphere.Application.Services;

public interface IConfigService
{
    Task<List<NotificationType>> GetNotificationTypes();
    Task<EmailConfiguration> GetEmailConfiguration();
}