using Atmosphere.Application.Services;
using MediatR;

namespace Atmosphere.Application.Configuration.Commands;

public class UpdateValidationRulesHandler : IRequestHandler<UpdateValidationRules>
{
    private readonly IConfigService _configService;

    public UpdateValidationRulesHandler(IConfigService configService)
    {
        _configService = configService;
    }

    public async Task<Unit> Handle(UpdateValidationRules request, CancellationToken cancellationToken)
    {
        await _configService.UpdateValidationRules(request.ReadingType, request.Rules);

        return Unit.Value;
    }
}