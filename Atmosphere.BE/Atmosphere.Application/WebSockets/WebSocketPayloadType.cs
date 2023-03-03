using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Atmosphere.Application.WebSockets;

public enum WebSocketPayloadType
{
    // Generic message type
    [EnumMember(Value = "stringArray")]
    StringArray = 0,

    [EnumMember(Value = "string")]
    String = 1,

    [EnumMember(Value = "binary")]
    Binary = 2,

    // App message type
    [EnumMember(Value = "notification")]
    Notification = 3,

    [EnumMember(Value = "reading")]
    Reading = 4,
}
