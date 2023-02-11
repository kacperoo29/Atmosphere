using System.ComponentModel.DataAnnotations;
using Atmosphere.Application.DTO;
using MediatR;

namespace Atmosphere.Application.Readings.Queries;

public class GetReadingsByDevice : IRequest<List<ReadingDto>>
{
    [Required]
    public Guid DeviceId { get; set; }
}
