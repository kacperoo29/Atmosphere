using Atmosphere.Core.Enums;
using Atmosphere.Core.Models;
using MediatR;

namespace Atmosphere.Application.Readings.Commands;

public class CreateReading : IRequest<Reading>
{
    public string DeviceAddress { get; init; }
    public decimal Value { get; init; }
    public ReadingType Type { get; init; }
}