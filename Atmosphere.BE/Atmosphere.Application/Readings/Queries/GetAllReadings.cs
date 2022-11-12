using Atmosphere.Core.Models;
using MediatR;

namespace Atmosphere.Application.Readings.Queries;

public class GetAllReadings : IRequest<IEnumerable<Reading>>
{
    public Guid DeviceId { get; init; }
}