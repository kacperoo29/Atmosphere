namespace Atmosphere.Application.Auth.Commands;

using System.Threading;
using System.Threading.Tasks;
using Atmoshpere.Application.Services;
using Atmosphere.Application.Services;
using Atmosphere.Core.Repositories;
using MediatR;

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
        var user = await _userService.GetByCredentialsAsync(request.Identifier, request.Key);

        return await _tokenService.GenerateToken(user);
    }
}
