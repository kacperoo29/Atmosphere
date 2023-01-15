using Atmosphere.Application.Services;
using MediatR;

namespace Atmosphere.Application.Configuration.Commands;

public class UpdateEmailConfigHandler : IRequestHandler<UpdateEmailConfig, EmailConfiguration>
{
    private readonly IConfigService _configService;

    public UpdateEmailConfigHandler(IConfigService configService)
    {
        _configService = configService;
    }

    public async Task<EmailConfiguration> Handle(
        UpdateEmailConfig request,
        CancellationToken cancellationToken
    )
    {
        var config = await _configService.GetEmailConfigurationAsync(cancellationToken);
        config.SmtpServer = request.SmtpServer ?? config.SmtpServer;
        config.SmtpPort = request.SmtpPort ?? config.SmtpPort;
        config.SmtpUsername = request.SmtpUsername ?? config.SmtpUsername;
        config.SmtpPassword = request.SmtpPassword ?? config.SmtpPassword;
        config.EmailAddress = request.EmailAddress ?? config.EmailAddress;
        config.ServerEmailAddress = request.ServerEmailAddress ?? config.ServerEmailAddress;

        await _configService.UpdateEmailConfigAsync(config, cancellationToken);

        return config;
    }
}