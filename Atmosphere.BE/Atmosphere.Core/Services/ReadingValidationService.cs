using System.ComponentModel.DataAnnotations;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using Atmosphere.Core.Rules;

namespace Atmosphere.Core.Services;

public class ReadingValidationService : IReadingValidationService
{
    private readonly IReadingRepository _readingRepository;
    private readonly IConfigurationRepository _configurationRepository;

    public ReadingValidationService(IReadingRepository readingRepository,
        IConfigurationRepository configurationRepository)
    {
        _readingRepository = readingRepository;
        _configurationRepository = configurationRepository;
    }

    protected override async Task<ValidationResult> CheckIfInRange(Reading reading)
    {
        var rule = RuleFactory.Create(reading.Type, _configurationRepository);

        return await rule.ValidateRange(reading);
    }

    protected override async Task<ValidationResult> CompareToPrevious(Reading reading)
    {
        var rule = RuleFactory.Create(reading.Type, _configurationRepository);
        var previous = await _readingRepository.GetPrevious(reading.DeviceId, reading.Type);

        return await rule.ValidateWithPrevious(reading, previous);
    }
}
