using System.ComponentModel.DataAnnotations;
namespace Atmosphere.Application.DTO;

public class ValidationRuleDto
{
    [Required]
    public string Message { get; set; }

    [Required]
    public string Condition { get; set; }
}
