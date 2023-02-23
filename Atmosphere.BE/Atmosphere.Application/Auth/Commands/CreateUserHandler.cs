using Atmosphere.Application.DTO;
using Atmosphere.Application.Services;
using Atmosphere.Core.Models;
using AutoMapper;
using MediatR;

namespace Atmosphere.Application.Auth.Commands;

public class CreateUserHandler : IRequestHandler<CreateUser, UserDto>
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public CreateUserHandler(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(CreateUser request, CancellationToken cancellationToken)
    {
        var user = User.Create(request.Email, request.Password);
        var res = await _userService.CreateUserAsync(user);

        return _mapper.Map<UserDto>(res);
    }
}
