using System.ComponentModel.DataAnnotations;
using Atmosphere.Application.Services;
using Atmosphere.Core.Models;

namespace Atmosphere.Services.Notifications;

public class NotificationService : INotificationService
{
    public Task Notify(Reading reading, IEnumerable<ValidationResult> validationResults)
    {
        return Task.CompletedTask;
    }
}