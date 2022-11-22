using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using AutoMapper;
using MediatR;

namespace Atmosphere.Application.Configuration.Queries;

public class GetConfigurationEntriesHandler : IRequestHandler<GetConfigurationEntries, Dictionary<string, object?>>
{
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IMapper _mapper;

    public GetConfigurationEntriesHandler(IConfigurationRepository configurationRepository, IMapper mapper)
    {
        _configurationRepository = configurationRepository;
        _mapper = mapper;
    }

    public async Task<Dictionary<string, object?>> Handle(GetConfigurationEntries request,
        CancellationToken cancellationToken)
    {
        var configurations = await _configurationRepository.GetEntiresAsync(x => request.Keys.Contains(x.Key));

        return _mapper.Map<Dictionary<string, object?>>(configurations);
    }
}