using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Atmosphere.Application.Configuration.Commands;

public class SetPollingRate : IRequest
{
    [Required]
    public int PollingRate { get; set; }

    public Guid? DeviceId { get; set; }
}