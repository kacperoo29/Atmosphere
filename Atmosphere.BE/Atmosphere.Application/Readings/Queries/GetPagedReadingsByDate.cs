using System.ComponentModel.DataAnnotations;
using Atmosphere.Application.DTO;
using Atmosphere.Core;
using MediatR;

namespace Atmosphere.Application.Readings.Queries;

public class GetPagedReadingsByDate : IRequest<PagedList<ReadingDto>>
{
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }

    [Required]
    public int PageNumber { get; init; }

    [Required]
    public int PageSize { get; init; }
}