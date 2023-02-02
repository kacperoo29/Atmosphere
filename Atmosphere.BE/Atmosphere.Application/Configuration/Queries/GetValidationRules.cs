using Atmosphere.Application.DTO;
using Atmosphere.Core.Enums;
using MediatR;

namespace Atmosphere.Application.Configuration.Queries;

public class GetValidationRules : IRequest<List<ValidationRuleDto>>
{
    public ReadingType ReadingType { get; init; }
}