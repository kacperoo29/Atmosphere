using System.ComponentModel.DataAnnotations;
using Atmosphere.Core.Enums;

namespace Atmosphere.Application.DTO;

public class ValidationRuleDto
{
    [Required]
    public string Message { get; set; }

    [Required]
    public string Condition { get; set; }

    [Required]
    public Severity Severity { get; set; }
}
