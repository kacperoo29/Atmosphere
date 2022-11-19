using System.ComponentModel.DataAnnotations;
using Atmosphere.Application.DTO;
using Atmosphere.Core.Enums;
using MediatR;

namespace Atmosphere.Application.Readings.Commands;

public class CreateReading : IRequest<ReadingDto>
{
    [Required]
    public string SensorIdentifier { get; init; }

    [Required]
    public decimal Value { get; init; }

    [Required]
    public ReadingType Type { get; init; }

    [Required]
    public DateTime Timestamp { get; init; }

    public CreateReading()
    {
        SensorIdentifier = string.Empty;
        Value = 0;
        Type = ReadingType.Temperature;
        Timestamp = DateTime.UtcNow;
    }
}
