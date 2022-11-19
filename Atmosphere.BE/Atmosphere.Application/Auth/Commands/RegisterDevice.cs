using System.ComponentModel.DataAnnotations;
using Atmosphere.Application.DTO;
using MediatR;

namespace Atmosphere.Application.Auth;

public class RegisterDevice : IRequest<DeviceDto>
{
    [Required]
    public string Username { get; init; }

    [Required]
    public string Identifier { get; init; }

    [Required]
    public string Password { get; init; }

    public RegisterDevice()
    {
        Username = string.Empty;
        Identifier = string.Empty;
        Password = string.Empty;
    }
}