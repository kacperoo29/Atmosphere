using Atmosphere.Application.Configuration;
using Atmosphere.Core.Enums;
using Atmosphere.Core.Models;

namespace Atmosphere.Application.Services;

public interface IConfigService
{
    Task<List<NotificationType>> GetNotificationTypesAsync(
        CancellationToken cancellationToken = default
    );
    Task UpdateNotificationTypesAsync(
        List<NotificationType> types,
        CancellationToken cancellationToken = default
    );
    Task<EmailConfiguration> GetEmailConfigurationAsync(
        CancellationToken cancellationToken = default
    );
    Task<NotificationSettings> GetNotificationSettingsAsync(
        CancellationToken cancellationToken = default
    );
    Task UpdateNotificationSettingsAsync(
        NotificationSettings settings,
        CancellationToken cancellationToken = default
    );
    Task UpdateEmailConfigAsync(EmailConfiguration config, CancellationToken cancellationToken);
}
