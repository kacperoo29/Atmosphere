using Atmosphere.Core.Repositories;
using MediatR;

namespace Atmosphere.Application.Configuration.Commands;

public class UpdateConfigurationHandler : IRequestHandler<UpdateConfiguration, object?>
{
    private readonly IConfigurationRepository _configurationRepository;

    public UpdateConfigurationHandler(IConfigurationRepository configurationRepository)
    {
        _configurationRepository = configurationRepository;
    }

    public async Task<object?> Handle(
        UpdateConfiguration request,
        CancellationToken cancellationToken
    )
    {
        await _configurationRepository.Set(request.Key, request.Value);

        return await _configurationRepository.Get(request.Key);
    }
}
