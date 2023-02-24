using Atmosphere.Application.DTO;
using MediatR;

namespace Atmosphere.Application.Auth.Queries;

public class GetUsers : IRequest<IEnumerable<UserDto>>
{
    public GetUsers()
    {
    }
}