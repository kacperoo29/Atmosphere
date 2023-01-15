using System.ComponentModel.DataAnnotations;
using Atmosphere.Core.Enums;
using Atmosphere.Core.Models;
using MediatR;

namespace Atmosphere.Application.Configuration.Commands;

public class ToggleNotificationType : IRequest<IEnumerable<NotificationType>>
{
    [Required]
    public NotificationType Type { get; init; }
}