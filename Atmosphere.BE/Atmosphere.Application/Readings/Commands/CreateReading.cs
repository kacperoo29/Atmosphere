namespace Atmosphere.Application.Readings.Commands;

using System;
using Atmosphere.Core.Enums;
using Atmosphere.Core.Models;
using MediatR;

public class CreateReading : IRequest<Reading>
{
    public decimal Value { get; init; }
    public DateTime Timestamp { get; init; }
    public ReadingType Type { get; init; }
}
