using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Atmosphere.Core.Enums;

namespace Atmosphere.Core.Models;

public class Reading : BaseModel
{
    protected Reading()
        : base()
    {
        Type = ReadingType.Unknown;
        Value = 0;
        Timestamp = DateTime.UtcNow;
    }

    public Guid DeviceId { get; private set; }
    public decimal Value { get; private set; }
    public DateTime Timestamp { get; private set; }
    public ReadingType Type { get; private set; }

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
}