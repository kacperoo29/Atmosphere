using System.Runtime.Serialization;

namespace Atmosphere.Core.Enums;

public enum NotificationType
{
    [EnumMember(Value = "unknown")]
    Unknown = -1,

    [EnumMember(Value = "email")]
    Email = 0,

    [EnumMember(Value = "webSocket")]
    WebSocket = 1,
}
