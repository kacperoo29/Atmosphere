using System.ComponentModel.DataAnnotations;
using Atmosphere.Core.Enums;

namespace Atmosphere.Application.DTO;

public class ReadingDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public DeviceDto Device { get; set; }

    [Required]
    public decimal Value { get; set; }

    [Required]
    public string Unit { get; set; }

    [Required]
    public DateTime Timestamp { get; set; }

    [Required]
    public ReadingType Type { get; set; }

    public ReadingDto()
    {
    }
}