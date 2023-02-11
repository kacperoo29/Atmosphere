using System.ComponentModel.DataAnnotations;

namespace Atmosphere.Application.DTO;

public class DeviceDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Identifier { get; set; }

    [Required]
    public bool IsActive { get; set; }

    [Required]
    public bool IsConnected { get; set; }

    public DeviceDto()
    {
        Identifier = string.Empty;
    }
}