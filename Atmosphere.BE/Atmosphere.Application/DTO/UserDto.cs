using System.ComponentModel.DataAnnotations;
using Atmosphere.Core.Enums;

namespace Atmosphere.Application.DTO;

public class UserDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public UserRole Role { get; set; }

    [Required]
    public bool IsActive { get; set; }
}
