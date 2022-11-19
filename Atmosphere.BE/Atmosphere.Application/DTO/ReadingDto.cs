using System.ComponentModel.DataAnnotations;
using Atmosphere.Core.Enums;

namespace Atmosphere.Application.DTO;

public class ReadingDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid DeviceId { get; set; }

    [Required]
    public string SensorIdentifier { get; set; }

    [Required]
    public double Value { get; set; }

    [Required]
    public DateTime Timestamp { get; set; }

    [Required]
    public ReadingType Type { get; set; }

    public ReadingDto()
    {
        SensorIdentifier = string.Empty;
    }
}