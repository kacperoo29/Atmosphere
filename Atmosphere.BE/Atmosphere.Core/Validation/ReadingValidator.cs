using System.ComponentModel.DataAnnotations;
using Atmosphere.Core.Enums;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;

namespace Atmosphere.Core.Validation;

public class ReadingValidator : IReadingValidator
{
    private const string ValidationRulesKey = "Atmosphere.Core.ValidationRules";

    public static string GetValidationRulesKey(Guid? deviceId = null)
    {
        return $"{ValidationRulesKey}{(deviceId.HasValue ? $".{deviceId.Value.ToString()}" : string.Empty)}";
    }

    private readonly IConfigurationRepository _configurationRepository;

    public ReadingValidator(IConfigurationRepository configurationRepository)
    {
        _configurationRepository = configurationRepository;
    }

    public Dictionary<ReadingType, List<ValidationRule>> GetDefaultRules()
    {
        var rules = new Dictionary<ReadingType, List<ValidationRule>>();
        rules.Add(
            ReadingType.Temperature,
            new List<ValidationRule>
            {
                new ValidationRule
                {
                    Message = "Temperature is below {severity} threshold of {value}",
                    Severity = Severity.Warning,
                    Condition = v => v.Value < 0
                },
                new ValidationRule
                {
                    Message = "Temperature is above {severity} threshold of {value}",
                    Severity = Severity.Warning,
                    Condition = v => v.Value > 100
                },
                new ValidationRule
                {
                    Message = "Temperature is below {severity} threshold of {value}",
                    Severity = Severity.Error,
                    Condition = v => v.Value < -10
                },
                new ValidationRule
                {
                    Message = "Temperature is above {severity} threshold of {value}",
                    Severity = Severity.Error,
                    Condition = v => v.Value > 110
                }
            }
        );

        return rules;
    }

    public async Task<IEnumerable<Notification>> Validate(Reading reading, Guid? deviceId = null)
    {
        var rules = await _configurationRepository.GetAsync<
            Dictionary<ReadingType, List<ValidationRule>>
        >(GetValidationRulesKey(deviceId));

        if (deviceId != null && rules == null)
        {
            rules = await _configurationRepository.GetAsync<
                Dictionary<ReadingType, List<ValidationRule>>
            >(GetValidationRulesKey());
        }

        var validationResults = new List<Notification>();
        if (rules == null)
        {
            rules = GetDefaultRules();
            await _configurationRepository.SetAsync(GetValidationRulesKey(), rules);
        }

        if (!rules.ContainsKey(reading.Type))
        {
            return validationResults;
        }

        foreach (var rule in rules[reading.Type])
        {
            if (rule.Condition.Compile()(reading))
            {
                var message = rule.Message.Replace(
                    "{value}",
                    reading.Value.ToString(),
                    StringComparison.OrdinalIgnoreCase
                );
                message = message.Replace(
                    "{severity}",
                    rule.Severity.ToString(),
                    StringComparison.OrdinalIgnoreCase
                );

                validationResults.Add(
                    new Notification { Severity = rule.Severity, Message = message }
                );
            }
        }

        // Keep only the highest severity
        if (validationResults.Count == 0)
        {
            return validationResults;
        }

        var highestSeverity = validationResults.Max(v => v.Severity);
        validationResults.RemoveAll(v => v.Severity < highestSeverity);

        return validationResults;
    }
}
