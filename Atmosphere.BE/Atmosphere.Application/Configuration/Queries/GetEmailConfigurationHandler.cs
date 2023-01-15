using Atmosphere.Application.Services;
using MediatR;

namespace Atmosphere.Application.Configuration.Queries;

public class GetEmailConfigurationHandler : IRequestHandler<GetEmailConfiguration, EmailConfiguration>
{
    private readonly IConfigService _configService;

    public GetEmailConfigurationHandler(IConfigService configService)
    {
        _configService = configService;
    }

    public async Task<EmailConfiguration> Handle(
        GetEmailConfiguration request,
        CancellationToken cancellationToken
    )
    {
        return await _configService.GetEmailConfigurationAsync(cancellationToken);
    }
}

