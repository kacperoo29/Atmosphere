namespace Atmosphere.Core.Rules;

using System.ComponentModel.DataAnnotations;

using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;

public class TemperatureRule : IValidationRule<Reading>
{
    private readonly IConfigurationRepository _configurationRepository;

    public const decimal DefaultMinTemperature = -10;
    public const string MinTemperature = "MinTemperature";

    public const decimal DefaultMaxTemperature = 30;
    public const string MaxTemperature = "MaxTemperature";

    public const decimal DefaultMaxDifference = 5;
    public const string MaxDifference = "MaxDifference";

    public TemperatureRule(IConfigurationRepository configurationRepository)
    {
        _configurationRepository = configurationRepository;
    }

    public async Task<ValidationResult> ValidateRange(Reading reading)
    {
        var minTemp = ((decimal?)await _configurationRepository.Get(MinTemperature)) ?? DefaultMinTemperature;
        var maxTemp = ((decimal?)await _configurationRepository.Get(MaxTemperature)) ?? DefaultMaxTemperature;

        if (reading.Type != ReadingType.Temperature)
        {
            return ValidationResult.Success!;
        }

        if (reading.Value < minTemp)
        {
            return new ValidationResult(
                $"Temperature dropped below {minTemp} and is now {reading.Value}.",
                new[] { nameof(Reading.Value) });
        }

        if (reading.Value > maxTemp)
        {
            return new ValidationResult(
                $"Temperature exceeded {maxTemp} and is now {reading.Value}.",
                new[] { nameof(Reading.Value) });
        }

        return ValidationResult.Success!;
    }

    public async Task<ValidationResult> ValidateWithPrevious(Reading reading, Reading previous)
    {
        var maxDiff = ((decimal?)await _configurationRepository.Get(MaxDifference)) ?? DefaultMaxDifference;

        if (reading.Type != ReadingType.Temperature)
        {
            return ValidationResult.Success!;
        }

        if (previous == null)
        {
            return ValidationResult.Success!;
        }

        if (reading.Value - previous.Value > maxDiff)
        {
            return new ValidationResult(
                $"Temperature difference exceeded {maxDiff} and is now {reading.Value - previous.Value}.",
                new[] { nameof(Reading.Value) });
        }

        return ValidationResult.Success!;
    }
}
