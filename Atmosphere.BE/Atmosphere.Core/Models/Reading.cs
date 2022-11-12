using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Atmosphere.Core.Enums;

namespace Atmosphere.Core.Models;

public class Reading : BaseModel
{
    private Reading()
    {
    }

    public Guid DeviceId { get; private set; }
    public string DeviceAddress { get; private set; }
    public decimal Value { get; private set; }
    public DateTime Timestamp { get; private set; }
    public ReadingType Type { get; private set; }

    public static Reading Create(Guid deviceId, string deviceAddress, decimal value, DateTime timestamp, ReadingType type)
    {
        return new Reading
        {
            DeviceId = deviceId,
            DeviceAddress = deviceAddress,
            Value = value,
            Timestamp = timestamp,
            Type = type
        };
    }
}