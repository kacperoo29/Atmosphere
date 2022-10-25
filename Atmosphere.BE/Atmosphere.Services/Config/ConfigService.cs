using Atmosphere.Application.Configuration;
using Atmosphere.Application.Services;
using Atmosphere.Core.Enums;
using Atmosphere.Core.Repositories;
using Microsoft.Extensions.Configuration;

namespace Atmosphere.Application.Config;

public class ConfigService : IConfigService
{
    private const string NOTIFICATION_TYPES_KEY = "notificationTypes";
    public const string EMAIL_CONFIG_KEY = "emailConfig";

    private readonly IConfigurationRepository _configRepo;
    private readonly IConfiguration _configuration;

    public ConfigService(IConfigurationRepository configRepo, IConfiguration configuration)
    {
        this._configRepo = configRepo;
        this._configuration = configuration;
    }

    public async Task<List<NotificationType>> GetNotificationTypes()
    {
        var types = await this._configRepo.Get(NOTIFICATION_TYPES_KEY);
        var typeList = types as List<NotificationType>;
        if (typeList is null) {
            typeList = new List<NotificationType>();
            await this._configRepo.Set(NOTIFICATION_TYPES_KEY, typeList);

            return typeList;
        }

        return typeList;
    }

    public async Task<EmailConfiguration> GetEmailConfiguration()
    {
        var emailConfig = await this._configRepo.Get(EMAIL_CONFIG_KEY) as EmailConfiguration;
        if (emailConfig is null) {
            var emailSection = this._configuration.GetSection("EmailSettings");
            emailConfig = new EmailConfiguration
            {
                SmtpServer = emailSection.GetValue<string>("SmtpServer"),
                SmtpPort = emailSection.GetValue<int>("SmtpPort"),
                SmtpUsername = emailSection.GetValue<string>("SmtpUsername"),
                SmtpPassword = emailSection.GetValue<string>("SmtpPassword"),
                EmailAddress = emailSection.GetValue<string>("EmailAddress"),
                ServerEmailAddress = emailSection.GetValue<string>("ServerEmailAddress")
            };
            
            await this._configRepo.Set(EMAIL_CONFIG_KEY, emailConfig);

            return emailConfig;
        }

        return emailConfig;
    }
}