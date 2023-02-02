using Atmosphere.Application.DTO;
using Atmosphere.Application.Services;
using AutoMapper;
using MediatR;

namespace Atmosphere.Application.Configuration.Queries;

public class GetValidationRulesHandler : IRequestHandler<GetValidationRules, List<ValidationRuleDto>>
{
    private readonly IConfigService _configService;
    private readonly IMapper _mapper;

    public GetValidationRulesHandler(IConfigService configService, IMapper mapper)
    {
        _configService = configService;
        _mapper = mapper;
    }

    public async Task<List<ValidationRuleDto>> Handle(GetValidationRules request, CancellationToken cancellationToken)
    {
        var rules =  await _configService.GetValidationRules(request.ReadingType);

        return _mapper.Map<List<ValidationRuleDto>>(rules);
    }
}