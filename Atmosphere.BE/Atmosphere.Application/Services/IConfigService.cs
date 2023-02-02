using Atmosphere.Application.Configuration;
using Atmosphere.Core.Enums;
using Atmosphere.Core.Validation;

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
    Task UpdateEmailConfigAsync(EmailConfiguration config, CancellationToken cancellationToken);
    Task<List<ValidationRule>> GetValidationRules(ReadingType readingType);
}
