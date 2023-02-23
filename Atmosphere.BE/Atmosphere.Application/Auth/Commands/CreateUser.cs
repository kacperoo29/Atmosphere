using Atmosphere.Application.DTO;
using MediatR;

namespace Atmosphere.Application.Auth.Commands;

public class CreateUser : IRequest<UserDto>
{
    public string Email { get; set; }
    public string Password { get; set; }
}