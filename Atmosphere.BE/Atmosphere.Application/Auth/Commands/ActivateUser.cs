using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Atmosphere.Application.Auth.Commands;

public class ActivateUser : IRequest
{
    [Required]
    public Guid Id { get; init; }
}