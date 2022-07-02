namespace Atmosphere.Core.Rules;

using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;

public class RuleFactory
{
    public static IValidationRule<Reading> Create(ReadingType type, IConfigurationRepository config)
    {
        switch (type)
        {
            case ReadingType.Temperature:
                return new TemperatureRule(config);
            // TODO: Add other rules here
            // case ReadingType.Humidity:
            //     return new HumidityRules();
            // case ReadingType.Pressure:
            //     return new PressureRules();
            default:
                throw new ArgumentException("Invalid reading type");
        }
    }
}