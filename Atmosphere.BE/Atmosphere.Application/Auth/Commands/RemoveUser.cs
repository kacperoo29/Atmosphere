using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Atmosphere.Application.Auth.Commands;

public class RemoveUser : IRequest
{
    [Required]
    public Guid Id { get; init; }
}
