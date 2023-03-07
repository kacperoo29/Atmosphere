using System.ComponentModel.DataAnnotations;
using Atmosphere.Application.DTO;
using Atmosphere.Core.Enums;
using MediatR;

namespace Atmosphere.Application.Readings.Queries;

public class GetChartData : IRequest<Dictionary<DateTime, decimal>>
{
    public Guid? DeviceId { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }

    [Required]
    public ReadingType ReadingType { get; init; }
}