using Atmosphere.Core.Repositories;
using MediatR;

namespace Atmosphere.Application.Auth.Commands;

public class RemoveUserHandler : IRequestHandler<RemoveUser>
{
    private readonly IUserRepository _userRepository;

    public RemoveUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(RemoveUser request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        await _userRepository.RemoveAsync(user);

        return Unit.Value;
    }
}
