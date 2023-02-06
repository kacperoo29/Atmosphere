using System.ComponentModel.DataAnnotations;
using Atmosphere.Application.DTO;
using Atmosphere.Core.Enums;
using MediatR;

namespace Atmosphere.Application.Configuration.Commands;

public class UpdateValidationRules : IRequest
{
    [Required]
    public ReadingType ReadingType { get; init; }

    [Required]
    public List<ValidationRuleDto> Rules { get; init; }
}