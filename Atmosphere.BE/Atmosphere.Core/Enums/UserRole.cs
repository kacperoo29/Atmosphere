using System.Runtime.Serialization;

namespace Atmosphere.Core.Enums;

public enum UserRole
{
    [EnumMember(Value = "user")] User = 0,

    [EnumMember(Value = "admin")] Admin = 1,

    [EnumMember(Value = "device")] Device = 2
}