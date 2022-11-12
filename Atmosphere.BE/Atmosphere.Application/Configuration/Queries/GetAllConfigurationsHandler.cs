using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using MediatR;

namespace Atmosphere.Application.Configuration.Queries;

public class GetAllConfigurationsHandler : IRequestHandler<GetAllConfigurations, IEnumerable<ConfigurationEntry>>
{
    private readonly IConfigurationRepository _configurationRepository;

    public GetAllConfigurationsHandler(IConfigurationRepository configurationRepository)
    {
        _configurationRepository = configurationRepository;
    }

    public async Task<IEnumerable<ConfigurationEntry>> Handle(GetAllConfigurations request,
        CancellationToken cancellationToken)
    {
        return await _configurationRepository.GetEntires();
    }
}