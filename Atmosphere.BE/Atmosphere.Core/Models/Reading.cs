namespace Atmosphere.Core.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text;
using Atmosphere.Core.Enums;

public class Reading : BaseModel
{
    public Guid DeviceId { get; private set; }
    public decimal Value { get; private set; }
    public DateTime Timestamp { get; private set; }
    public ReadingType Type { get; private set; }

    private Reading()
        : base()
    {
    }

    public static Reading Create(Guid deviceId, decimal value, DateTime timestamp, ReadingType type)
    {
        return new Reading
        {
            DeviceId = deviceId,
            Value = value,
            Timestamp = timestamp,
            Type = type
        };
    }

    public IEnumerable<Expression<Func<Reading, ValidationResult?>>> ValidationRules()
    {
        switch (this.Type)
        {
            case ReadingType.Temperature:
                yield return (r) => r.Value < -100 || r.Value > 100 ?
                    new ValidationResult("Temperature must be between -100 and 100") :
                    ValidationResult.Success;

                break;
            case ReadingType.Humidity:
                yield return (r) => r.Value < 0 || r.Value > 100 ?
                    new ValidationResult("Humidity must be between 0 and 100") :
                    ValidationResult.Success;
                break;
            case ReadingType.Pressure:
                yield return (r) => r.Value < 0 || r.Value > 100 ?
                    new ValidationResult("Pressure must be between 0 and 100") :
                    ValidationResult.Success;
                break;
            default:
                yield return (r) => new ValidationResult("Unknown reading type");
                break;
        }
    }
}