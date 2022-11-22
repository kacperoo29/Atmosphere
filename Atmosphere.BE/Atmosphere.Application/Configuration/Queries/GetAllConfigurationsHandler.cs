using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using AutoMapper;
using MediatR;

namespace Atmosphere.Application.Configuration.Queries;

public class GetAllConfigurationsHandler : IRequestHandler<GetAllConfigurations, Dictionary<string, object?>>
{
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IMapper _mapper;

    public GetAllConfigurationsHandler(IConfigurationRepository configurationRepository, IMapper mapper)
    {
        _configurationRepository = configurationRepository;
        _mapper = mapper;
    }

    public async Task<Dictionary<string, object?>> Handle(GetAllConfigurations request,
        CancellationToken cancellationToken)
    {
        var configurations =  await _configurationRepository.GetEntiresAsync();

        return _mapper.Map<Dictionary<string, object?>>(configurations);
    }
}