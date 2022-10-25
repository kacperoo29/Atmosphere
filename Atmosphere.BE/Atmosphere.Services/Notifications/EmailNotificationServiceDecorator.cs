namespace Atmosphere.Services.Notifications;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Atmosphere.Application.Services;
using Atmosphere.Core.Models;

using MailKit.Net.Smtp;
using MimeKit;

public class EmailNotificationServiceDecorator : INotificationService
{
    private readonly IConfigService _configService;
    private readonly INotificationService _wrapee;

    public EmailNotificationServiceDecorator(INotificationService wrapee, IConfigService configService)
    {
        _configService = configService;
        _wrapee = wrapee;
    }

    public async Task Notify(Reading reading, List<ValidationResult> validationResults)
    {
        await this._wrapee.Notify(reading, validationResults);

        var config = await this._configService.GetEmailConfiguration();

        var emailAddress = config.EmailAddress;
        var serverEmailAddress = config.ServerEmailAddress;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Atmosphere", config.ServerEmailAddress));
        message.To.Add(new MailboxAddress("Atmosphere", config.EmailAddress));
        message.Subject = $"Atmosphere reading";

        // TODO: Make more sensible email body
        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = $"<p>{reading.Value}</p>";
        foreach (var result in validationResults)
        {
            bodyBuilder.HtmlBody += $"<p>{result.ErrorMessage}</p>";
        }
        message.Body = bodyBuilder.ToMessageBody();

        var smtpServerAddress = config.SmtpServer;
        var smtpServerPort = config.SmtpPort;
        var smtpServerUsername = config.SmtpUsername;
        var smtpServerPassword = config.SmtpPassword;

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(smtpServerAddress, smtpServerPort, false);
            await client.AuthenticateAsync(smtpServerUsername, smtpServerPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}