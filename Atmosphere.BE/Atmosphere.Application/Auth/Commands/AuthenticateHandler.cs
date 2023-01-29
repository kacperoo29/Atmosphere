using Atmosphere.Application.Services;
using MediatR;

namespace Atmosphere.Application.Auth.Commands;

public class AuthenticateHandler : IRequestHandler<Authenticate, string>
{
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;

    public AuthenticateHandler(ITokenService tokenService, IUserService userRepository)
    {
        _tokenService = tokenService;
        _userService = userRepository;
    }

    public async Task<string> Handle(Authenticate request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetByCredentialsAsync(request.Username, request.Password);

        return await _tokenService.GenerateToken(user);
    }
}