using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Atmosphere.Application.Auth.Commands;

public class Authenticate : IRequest<string>
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