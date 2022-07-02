namespace Atmosphere.Application.Configuration.Queries;

using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;

using MediatR;

public class GetConfigurationEntriesHandler : IRequestHandler<GetConfigurationEntries, IEnumerable<ConfigurationEntry>>
{
    private readonly IConfigurationRepository _configurationRepository;

    public GetConfigurationEntriesHandler(IConfigurationRepository configurationRepository)
    {
        _configurationRepository = configurationRepository;
    }

    public async Task<IEnumerable<ConfigurationEntry>> Handle(GetConfigurationEntries request, CancellationToken cancellationToken)
    {
        return await _configurationRepository.GetEntires(x => request.Keys.Contains(x.Key));
    }
}