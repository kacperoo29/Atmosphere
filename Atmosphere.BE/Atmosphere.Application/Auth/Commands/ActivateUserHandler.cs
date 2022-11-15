using Atmosphere.Core.Repositories;
using MediatR;

namespace Atmosphere.Application.Auth.Commands;

public class ActivateUserHandler : IRequestHandler<ActivateUser>
{
    private readonly IUserRepository _userRepository;

    public ActivateUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(ActivateUser request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        user.Activate();

        await _userRepository.UpdateAsync(user);

        return Unit.Value;
    }
}