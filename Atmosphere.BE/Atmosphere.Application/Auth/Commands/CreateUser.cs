using System.ComponentModel.DataAnnotations;
using Atmosphere.Application.DTO;
using MediatR;

namespace Atmosphere.Application.Auth.Commands;

public class CreateUser : IRequest<UserDto>
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}