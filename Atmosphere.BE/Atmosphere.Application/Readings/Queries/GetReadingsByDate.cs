namespace Atmosphere.Application.Readings.Queries;

using Atmosphere.Application.DTO;
using MediatR;

public class GetReadingsByDate : IRequest<List<ReadingDto>>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public GetReadingsByDate()
    {
        StartDate = DateTime.MinValue;
        EndDate = DateTime.MaxValue;
    }
}