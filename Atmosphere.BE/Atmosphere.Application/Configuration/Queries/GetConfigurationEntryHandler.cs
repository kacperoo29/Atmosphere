using Atmosphere.Core.Repositories;

using MediatR;

namespace Atmosphere.Application.Configuration.Queries;

public class GetConfigurationEntryHandler : IRequestHandler<GetConfigurationEntry, object?>
{
    private readonly IConfigurationRepository _configurationRepository;

    public GetConfigurationEntryHandler(IConfigurationRepository configurationRepository)
    {
        _configurationRepository = configurationRepository;
    }

    public async Task<object?> Handle(GetConfigurationEntry request, CancellationToken cancellationToken)
    {
        if (request.Key == null)
            return null;
        
        return await _configurationRepository.Get(request.Key);
    }
}