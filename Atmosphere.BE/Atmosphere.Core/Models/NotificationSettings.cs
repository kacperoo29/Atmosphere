using Atmosphere.Core.Consts;

namespace Atmosphere.Core.Models
{
    public class NotificationSettings
    {
        public bool EmailEnabled { get; set; }
        public string EmailTo { get; set; }
        public decimal TemperatureThresholdMin { get; set; }
        public decimal TemperatureThresholdMax { get; set; }

        public NotificationSettings()
        {
            EmailEnabled = true;
            EmailTo = string.Empty;
            TemperatureThresholdMin = 0;
            TemperatureThresholdMax = 100;
        }
    }
}