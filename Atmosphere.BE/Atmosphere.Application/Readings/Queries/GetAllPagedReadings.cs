using System.ComponentModel.DataAnnotations;
using Atmosphere.Application.DTO;
using Atmosphere.Core;
using MediatR;

namespace Atmosphere.Application.Readings.Queries;

public class GetAllPagedReadings : IRequest<PagedList<ReadingDto>>
{
    [Required]
    public int PageNumber { get; init; }

    [Required]
    public int PageSize { get; init; }
}