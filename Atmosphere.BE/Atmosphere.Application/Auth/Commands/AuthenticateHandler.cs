namespace Atmosphere.Application.Auth.Commands;

using System.Threading;
using System.Threading.Tasks;
using Atmosphere.Application.Services;
using Atmosphere.Core.Repositories;
using MediatR;

public class AuthenticateHandler : IRequestHandler<Authenticate, string>
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public AuthenticateHandler(ITokenService tokenService, IUserRepository userRepository)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    public async Task<string> Handle(Authenticate request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByCredentialsAsync(request.Identifier, request.Key);

        return await _tokenService.GenerateToken(user);
    }
}
