using System.Linq.Expressions;
using Atmosphere.Application.Configuration;
using Atmosphere.Application.DTO;
using Atmosphere.Application.Services;
using Atmosphere.Core.Enums;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using Atmosphere.Core.Validation;
using Atmosphere.Services.Exceptions;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.Configuration;

namespace Atmosphere.Services.Config;

public class ConfigService : IConfigService
{
    public const string NOTIFICATION_TYPES_KEY = "Atmosphere.Notification.Types";
    public const string NOTIFICATIONS_SETTINGS_KEY = "Atmosphere.Notification.Settings";
    public const string EMAIL_CONFIG_KEY = "Atmosphere.Notification.EmailConfig";

    private readonly IConfigurationRepository _configRepo;
    private readonly IConfiguration _configuration;

    public ConfigService(IConfigurationRepository configRepo, IConfiguration configuration)
    {
        _configRepo = configRepo;
        _configuration = configuration;
    }

    public async Task<List<NotificationType>> GetNotificationTypesAsync(
        CancellationToken cancellationToken = default
    )
    {
        var types = await _configRepo.GetAsync<List<NotificationType>>(
            NOTIFICATION_TYPES_KEY,
            cancellationToken
        );
        if (types is null)
        {
            types = new List<NotificationType>();
            await _configRepo.SetAsync(NOTIFICATION_TYPES_KEY, types, cancellationToken);

            return types;
        }

        return types;
    }

    public async Task<EmailConfiguration> GetEmailConfigurationAsync(
        CancellationToken cancellationToken
    )
    {
        var emailConfig = await _configRepo.GetAsync<EmailConfiguration>(
            EMAIL_CONFIG_KEY,
            cancellationToken
        );
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

    public async Task UpdateNotificationTypesAsync(
        List<NotificationType> types,
        CancellationToken cancellationToken = default
    )
    {
        await _configRepo.SetAsync(NOTIFICATION_TYPES_KEY, types, cancellationToken);
    }

    public Task UpdateEmailConfigAsync(
        EmailConfiguration config,
        CancellationToken cancellationToken
    )
    {
        return _configRepo.SetAsync(EMAIL_CONFIG_KEY, config, cancellationToken);
    }

    public async Task<List<ValidationRule>> GetValidationRules(ReadingType readingType, Guid? deviceId = null)
    {
        var rules = await _configRepo.GetAsync<Dictionary<ReadingType, List<ValidationRule>>>(
            ReadingValidator.GetValidationRulesKey(deviceId)
        );

        if (rules is null)
        {
            rules = new Dictionary<ReadingType, List<ValidationRule>>();
            await _configRepo.SetAsync(ReadingValidator.GetValidationRulesKey(deviceId), rules);
        }

        if (!rules.ContainsKey(readingType))
        {
            rules.Add(readingType, new List<ValidationRule>());
            await _configRepo.SetAsync(ReadingValidator.GetValidationRulesKey(deviceId), rules);
        }

        return rules[readingType];
    }

    public async Task UpdateValidationRules(ReadingType readingType, List<ValidationRuleDto> rules, Guid? deviceId = null)
    {
        var oldRules = await _configRepo.GetAsync<Dictionary<ReadingType, List<ValidationRule>>>(
            ReadingValidator.GetValidationRulesKey(deviceId)
        );

        if (oldRules is null)
        {
            oldRules = new Dictionary<ReadingType, List<ValidationRule>>();
        }

        if (!oldRules.ContainsKey(readingType))
        {
            oldRules.Add(readingType, new List<ValidationRule>());
        }

        var newRules = new List<ValidationRule>();
        foreach (var rule in rules)
        {
            try
            {
                var options = ScriptOptions.Default.AddReferences(typeof(Reading).Assembly);
                var expression = await CSharpScript.EvaluateAsync<Expression<Func<Reading, bool>>>(
                    rule.Condition,
                    options
                );

                newRules.Add(
                    new ValidationRule
                    {
                        Condition = expression,
                        Message = rule.Message,
                        Severity = rule.Severity
                    }
                );
            }
            catch (CompilationErrorException e)
            {
                throw new InvalidRuleException(e.Message.Split(": ").Last());
            }
        }

        oldRules[readingType] = newRules;

        await _configRepo.SetAsync(ReadingValidator.GetValidationRulesKey(deviceId), oldRules);
    }
}
