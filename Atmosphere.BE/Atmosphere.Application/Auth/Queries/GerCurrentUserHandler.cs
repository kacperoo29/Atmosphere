using Atmosphere.Application.DTO;
using Atmosphere.Application.Services;
using AutoMapper;
using MediatR;

namespace Atmosphere.Application.Auth.Queries;

public class GetCurrentUserHandler : IRequestHandler<GetCurrentUser, BaseUserDto>
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public GetCurrentUserHandler(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<BaseUserDto> Handle(GetCurrentUser request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetCurrentAsync();
        if (user == null)
        {
            throw new UnauthorizedAccessException("User is not authorized");
        }

        return _mapper.Map<BaseUserDto>(user);
    }
}