namespace Atmosphere.Application.Readings.Commands;

using System;

using Atmosphere.Core.Models;

using MediatR;

public class CreateReading : IRequest<Reading>
{
    public Guid DeviceId { get; init; }
    public double Value { get; init; }
    public DateTime Timestamp { get; init; }
    public ReadingType Type { get; init; }
}
