using Atmosphere.Core.Enums;

namespace Atmosphere.Core.Models;

public class Notification
{
    public string Message { get; set; }
    public Severity Severity { get; set; }
}