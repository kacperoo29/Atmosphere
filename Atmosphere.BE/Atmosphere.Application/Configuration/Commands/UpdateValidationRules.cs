using Atmosphere.Application.DTO;
using Atmosphere.Core.Enums;
using MediatR;

namespace Atmosphere.Application.Configuration.Commands;

public class UpdateValidationRules : IRequest
{
    public ReadingType ReadingType { get; init; }
    public List<ValidationRuleDto> Rules { get; init; }
}