namespace Atmosphere.Core.Models;

using System;

public class Reading : BaseModel
{
    public Guid DeviceId { get; private set; }
    public double Value { get; private set; }
    public DateTime Timestamp { get; private set; }
    public ReadingType Type { get; private set; }

    private Reading()
        : base()
    {
    }

    public static Reading Create(Guid deviceId, double value, DateTime timestamp, ReadingType type)
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