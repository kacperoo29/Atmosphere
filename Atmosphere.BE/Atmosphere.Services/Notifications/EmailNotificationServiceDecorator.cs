using System.ComponentModel.DataAnnotations;
using Atmosphere.Application.Services;
using Atmosphere.Core.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace Atmosphere.Services.Notifications;

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
        await _wrapee.Notify(reading, validationResults);

        var config = await _configService.GetEmailConfigurationAsync();

        var emailAddress = config.EmailAddress;
        var serverEmailAddress = config.ServerEmailAddress;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Atmosphere", config.ServerEmailAddress));
        message.To.Add(new MailboxAddress("Atmosphere", config.EmailAddress));
        message.Subject = "Atmosphere reading";

        // TODO: Make more sensible email body
        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = $"<p>{reading.Value}</p>";
        foreach (var result in validationResults) bodyBuilder.HtmlBody += $"<p>{result.ErrorMessage}</p>";
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