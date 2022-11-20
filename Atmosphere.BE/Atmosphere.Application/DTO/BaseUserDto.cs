using System.ComponentModel.DataAnnotations;
using Atmosphere.Core.Enums;

namespace Atmosphere.Application.DTO;

public class BaseUserDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public UserRole Role { get; set; }

    [Required]
    public bool IsActive { get; set; }

    public BaseUserDto()
    {
        this.Username = string.Empty;
    }
}
