using Atmosphere.Application.DTO;
using MediatR;

namespace Atmosphere.Application.Auth.Queries;

public class GetCurrentUser : IRequest<BaseUserDto> { }
