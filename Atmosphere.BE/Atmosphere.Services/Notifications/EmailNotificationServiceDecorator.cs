using System.ComponentModel.DataAnnotations;
using Atmosphere.Application.Services;
using Atmosphere.Core.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace Atmosphere.Services.Notifications;

public class EmailNotificationServiceDecorator : NotificationService
{
    private readonly IConfigService _configService;

    public EmailNotificationServiceDecorator(
        INotificationService wrapee,
        IConfigService configService
    ) : base(wrapee)
    {
        _configService = configService;
    }

    public override async Task Notify(Reading reading, IEnumerable<Notification> validationResults)
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
        foreach (var result in validationResults)
            bodyBuilder.HtmlBody += $"<p>{result.Message}</p>";
        message.Body = bodyBuilder.ToMessageBody();

        var smtpServerAddress = config.SmtpServer;
        var smtpServerPort = config.SmtpPort;
        var smtpServerUsername = config.SmtpUsername;
        var smtpServerPassword = config.SmtpPassword;

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(smtpServerAddress, smtpServerPort ?? 587, false);
            if (!string.IsNullOrEmpty(smtpServerUsername))
                await client.AuthenticateAsync(smtpServerUsername, smtpServerPassword);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
