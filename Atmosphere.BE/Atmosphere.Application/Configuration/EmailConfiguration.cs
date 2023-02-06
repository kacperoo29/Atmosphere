using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Atmosphere.Application.Configuration;

public class EmailConfiguration : IRequest<EmailConfiguration>
{
    public string? SmtpServer { get; set; }
    public int? SmtpPort { get; set; }
    public string? SmtpUsername { get; set; }
    public string? SmtpPassword { get; set; }
    public string? EmailAddress { get; set; }
    public string? ServerEmailAddress { get; set; }
}
