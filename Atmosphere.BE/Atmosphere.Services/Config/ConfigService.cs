using Atmosphere.Application.Configuration;
using Atmosphere.Application.Services;
using Atmosphere.Core.Enums;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using Microsoft.Extensions.Configuration;

namespace Atmosphere.Application.Config;

public class ConfigService : IConfigService
{
    private const string NOTIFICATION_TYPES_KEY = "notificationTypes";
    private const string NOTIFICATIONS_SETTINGS_KEY = "notificationsSettings";
    public const string EMAIL_CONFIG_KEY = "emailConfig";

    private readonly IConfigurationRepository _configRepo;
    private readonly IConfiguration _configuration;

    public ConfigService(IConfigurationRepository configRepo, IConfiguration configuration)
    {
        _configRepo = configRepo;
        _configuration = configuration;
    }

    public async Task<List<NotificationType>> GetNotificationTypesAsync(CancellationToken cancellationToken = default)
    {
        var types = await _configRepo.GetAsync<List<NotificationType>>(NOTIFICATION_TYPES_KEY, cancellationToken);
        if (types is null)
        {
            types = new List<NotificationType>();
            await _configRepo.SetAsync(NOTIFICATION_TYPES_KEY, types, cancellationToken);

            return types;
        }

        return types;
    }

    public async Task<EmailConfiguration> GetEmailConfigurationAsync(CancellationToken cancellationToken)
    {
        var emailConfig = await _configRepo.GetAsync<EmailConfiguration>(EMAIL_CONFIG_KEY, cancellationToken);
        if (emailConfig is null)
        {
            var emailSection = _configuration.GetSection("EmailSettings");
            emailConfig = new EmailConfiguration
            {
                SmtpServer = emailSection.GetValue<string>("SmtpServer"),
                SmtpPort = emailSection.GetValue<int>("SmtpPort"),
                SmtpUsername = emailSection.GetValue<string>("SmtpUsername"),
                SmtpPassword = emailSection.GetValue<string>("SmtpPassword"),
                EmailAddress = emailSection.GetValue<string>("EmailAddress"),
                ServerEmailAddress = emailSection.GetValue<string>("ServerEmailAddress")
            };

            await _configRepo.SetAsync(EMAIL_CONFIG_KEY, emailConfig, cancellationToken);

            return emailConfig;
        }

        return emailConfig;
    }

    public async Task<NotificationSettings> GetNotificationSettingsAsync(CancellationToken cancellationToken = default)
    {
        var settings = await _configRepo.GetAsync<NotificationSettings>(NOTIFICATIONS_SETTINGS_KEY, cancellationToken);
        if (settings is null)
        {
            settings = new NotificationSettings();
            await _configRepo.SetAsync(NOTIFICATIONS_SETTINGS_KEY, settings, cancellationToken);
        }

        return settings;
    }

    public async Task UpdateNotificationSettingsAsync(NotificationSettings settings, CancellationToken cancellationToken = default)
    {
        await _configRepo.SetAsync(NOTIFICATIONS_SETTINGS_KEY, settings, cancellationToken);
    }

    public async Task UpdateNotificationTypesAsync(List<NotificationType> types, CancellationToken cancellationToken = default)
    {
        await _configRepo.SetAsync(NOTIFICATION_TYPES_KEY, types, cancellationToken);
    }

    public Task UpdateEmailConfigAsync(EmailConfiguration config, CancellationToken cancellationToken)
    {
        return _configRepo.SetAsync(EMAIL_CONFIG_KEY, config, cancellationToken);
    }
}