using System.ComponentModel.DataAnnotations;
using Atmosphere.Application.DTO;
using Atmosphere.Core.Enums;
using MediatR;

namespace Atmosphere.Application.Configuration.Queries;

public class GetValidationRules : IRequest<List<ValidationRuleDto>>
{
    [Required]
    public ReadingType ReadingType { get; init; }

    public Guid? DeviceId { get; init; }
}