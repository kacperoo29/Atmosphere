using Atmosphere.Application.DTO;
using MediatR;

namespace Atmosphere.Application.Readings.Queries;

public class GetAllReadings : IRequest<IEnumerable<ReadingDto>>
{
}