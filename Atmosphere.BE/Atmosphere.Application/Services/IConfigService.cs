using Atmosphere.Application.Configuration;
using Atmosphere.Core.Enums;
using Atmosphere.Core.Models;

namespace Atmosphere.Application.Services;

public interface IConfigService
{
    Task<List<NotificationType>> GetNotificationTypesAsync();
    Task<EmailConfiguration> GetEmailConfigurationAsync();
    Task<NotificationSettings> GetNotificationSettingsAsync(
        CancellationToken cancellationToken = default
    );
    Task UpdateNotificationSettingsAsync(
        NotificationSettings settings,
        CancellationToken cancellationToken = default
    );
}
