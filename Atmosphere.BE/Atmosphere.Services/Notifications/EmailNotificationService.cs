namespace Atmosphere.Services.Notifications;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Atmosphere.Application.Services;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;

using MailKit.Net.Smtp;
using MimeKit;

public class EmailNotificationService : INotificationService
{
    private readonly IConfigurationRepository _configurationRepository;

    public const string EMAIL_CONFIG_KEY = "emailConfig";

    public EmailNotificationService(IConfigurationRepository configurationRepository)
    {
        _configurationRepository = configurationRepository;
    }

    public async Task Notify(Reading reading, List<ValidationResult> validationResults)
    {
        var config = await _configurationRepository.Get(EMAIL_CONFIG_KEY) as EmailConfiguration
            ?? throw new Exception("Email configuration not found");

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