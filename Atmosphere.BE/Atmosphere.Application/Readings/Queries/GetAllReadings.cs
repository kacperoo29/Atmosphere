namespace Atmosphere.Application.Readings.Queries;

using Atmosphere.Core.Models;

using MediatR;

public class GetAllReadings : IRequest<IEnumerable<Reading>>
{
    public Guid DeviceId { get; init; }
}