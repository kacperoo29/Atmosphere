using System.ComponentModel.DataAnnotations;
using Atmosphere.Application.DTO;
using Atmosphere.Core;
using MediatR;

namespace Atmosphere.Application.Readings.Queries;

public class GetPagedReadingsByDevice : IRequest<PagedList<ReadingDto>>
{  
    [Required]
    public int PageNumber { get; init; }

    [Required]
    public int PageSize { get; init; }

    [Required]
    public Guid DeviceId { get; init; }

    public DateTime? StartDate { get; init; }

    public DateTime? EndDate { get; init; }
}