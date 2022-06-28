namespace Atmosphere.Core.Models;

using System.Runtime.Serialization;

public enum ReadingType
{
    [EnumMember(Value = "temperature")]
    Temperature = 0,

    [EnumMember(Value = "humidity")]
    Humidity = 1,

    [EnumMember(Value = "pressure")]
    Pressure = 2
}