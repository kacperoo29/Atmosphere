namespace Atmosphere.Infrastructure.Services;

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

    public EmailNotificationService(IConfigurationRepository configurationRepository)
    {
        _configurationRepository = configurationRepository;
    }

    public async Task Notify(Reading reading, List<ValidationResult> validationResults)
    {
        var emailAddress = await _configurationRepository.Get("emailAddress");
        var serverEmailAddress = await _configurationRepository.Get("serverEmailAddress");

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Atmosphere", serverEmailAddress));
        message.To.Add(new MailboxAddress("Atmosphere", emailAddress));
        message.Subject = $"Atmosphere reading";

        // TODO: Make more sensible email body
        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = $"<p>{reading.Value}</p>";
        foreach (var result in validationResults)
        {
            bodyBuilder.HtmlBody += $"<p>{result.ErrorMessage}</p>";
        }
        message.Body = bodyBuilder.ToMessageBody();

        var smtpServerAddress = await _configurationRepository.Get("smtpServerAddress");
        var smtpServerPort = int.Parse(await _configurationRepository.Get("smtpServerPort"));
        var smtpServerUsername = await _configurationRepository.Get("smtpServerUsername");
        var smtpServerPassword = await _configurationRepository.Get("smtpServerPassword");

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(smtpServerAddress, smtpServerPort, false);
            await client.AuthenticateAsync(smtpServerUsername, smtpServerPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}