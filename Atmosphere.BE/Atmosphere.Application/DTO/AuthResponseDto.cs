using System.ComponentModel.DataAnnotations;

namespace Atmosphere.Application.DTO;

public class AuthResponseDto
{
    [Required]
    public string Token { get; set; }
}
