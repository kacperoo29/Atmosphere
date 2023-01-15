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

    public async Task<IEnumerable<ValidationResult>> Validate(Reading reading)
    {
        var rules = await _configurationRepository.GetAsync<
            Dictionary<ReadingType, List<ValidationRule>>
        >(ValidationRulesKey);

        var validationResults = new List<ValidationResult>();
        if (rules == null)
        {
            // create defaults
            rules = new Dictionary<ReadingType, List<ValidationRule>>();
            rules.Add(ReadingType.Temperature, new List<ValidationRule>
            {
                new ValidationRule
                {
                    Message = "Temperature is below warning threshold of 0",
                    Value = 0,
                    Condition = v => v.Value < 0
                },
                new ValidationRule
                {
                    Message = "Temperature is above warning threshold of 100",
                    Value = 100,
                    Condition = v => v.Value > 100
                },
                new ValidationRule
                {
                    Message = "Temperature is below error threshold of -10",
                    Value = -10,
                    Condition = v => v.Value < -10
                },
                new ValidationRule
                {
                    Message = "Temperature is above error threshold of 110",
                    Value = 110,
                    Condition = v => v.Value > 110
                }
            });

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
                validationResults.Add(new ValidationResult(rule.Message));
            }
        }

        return validationResults;
    }
}
