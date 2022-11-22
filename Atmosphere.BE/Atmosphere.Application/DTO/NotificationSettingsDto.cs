using System.ComponentModel.DataAnnotations;

namespace Atmosphere.Application.DTO;

public class NotificationSettingsDto
{
    [Required]
    public bool EmailEnabled { get; set; }

    [Required]
    public string EmailTo { get; set; }

    [Required]
    public decimal TemperatureThresholdMin { get; set; }

    [Required]
    public decimal TemperatureThresholdMax { get; set; }

    public NotificationSettingsDto()
    {
        EmailTo = string.Empty;
    }
}