using System.ComponentModel.DataAnnotations;
using Atmosphere.Application.DTO;
using MediatR;

namespace Atmosphere.Application.Auth.Commands;

public class Authenticate : IRequest<AuthResponseDto>
{
    [Required]
    public string Username { get; init; }

    [Required]
    public string Password { get; init; }

    public Authenticate()
    {
        Username = string.Empty;
        Password = string.Empty;
    }
}