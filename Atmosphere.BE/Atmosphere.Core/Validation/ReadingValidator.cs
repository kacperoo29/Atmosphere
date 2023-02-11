using System.ComponentModel.DataAnnotations;
using Atmosphere.Core.Enums;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;

namespace Atmosphere.Core.Validation;

public class ReadingValidator : IReadingValidator
{
    public const string ValidationRulesKey = "Atmosphere.Core.ValidationRules";

    private readonly IConfigurationRepository _configurationRepository;

    public ReadingValidator(IConfigurationRepository configurationRepository)
    {
        _configurationRepository = configurationRepository;
    }

    public async Task<IEnumerable<Notification>> Validate(Reading reading)
    {
        var rules = await _configurationRepository.GetAsync<
            Dictionary<ReadingType, List<ValidationRule>>
        >(ValidationRulesKey);

        var validationResults = new List<Notification>();
        if (rules == null)
        {
            // create defaults
            rules = new Dictionary<ReadingType, List<ValidationRule>>();
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

            await _configurationRepository.SetAsync(ValidationRulesKey, rules);
        }

        if (!rules.ContainsKey(reading.Type))
        {
            return validationResults;
        }

        foreach (var rule in rules[reading.Type])
        {
            if (rule.Condition.Compile()(reading))
            {
                var message = rule.Message.Replace("{value}", reading.Value.ToString(), StringComparison.OrdinalIgnoreCase);
                message = message.Replace("{severity}", rule.Severity.ToString(), StringComparison.OrdinalIgnoreCase);

                validationResults.Add(
                    new Notification { Severity = rule.Severity, Message = message }
                );
            }
        }

        return validationResults;
    }
}
