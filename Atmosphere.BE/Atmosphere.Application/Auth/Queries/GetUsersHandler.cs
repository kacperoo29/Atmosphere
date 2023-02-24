using Atmosphere.Application.DTO;
using Atmosphere.Core.Enums;
using Atmosphere.Core.Repositories;
using AutoMapper;
using MediatR;

namespace Atmosphere.Application.Auth.Queries;

public class GetUsersHandler : IRequestHandler<GetUsers, IEnumerable<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUsersHandler(IUserRepository authRepository, IMapper mapper)
    {
        _userRepository = authRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> Handle(
        GetUsers request,
        CancellationToken cancellationToken
    )
    {
        var users = await _userRepository.GetAllAsync(x => x.Role == UserRole.User);

        return _mapper.Map<IEnumerable<UserDto>>(users);
    }
}
