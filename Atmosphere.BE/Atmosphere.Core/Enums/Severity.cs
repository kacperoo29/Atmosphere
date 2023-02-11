using System.Runtime.Serialization;

namespace Atmosphere.Core.Enums;

public enum Severity
{
    [EnumMember(Value = "info")]
    Info,

    [EnumMember(Value = "warning")]
    Warning,

    [EnumMember(Value = "error")]
    Error
}
