using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Atmosphere.Application.Configuration.Commands;

public class UpdateNotificationSettings : IRequest
{
    [Required]
    public bool EmailEnabled { get; init; }

    [Required]
    public string EmailTo { get; init; }

    [Required]
    public decimal TemperatureThresholdMin { get; init; }

    [Required]
    public decimal TemperatureThresholdMax { get; init; }
    

    public UpdateNotificationSettings()
    {
        EmailTo = string.Empty;
    }
}
