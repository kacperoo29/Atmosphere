using System.ComponentModel.DataAnnotations;
using Atmosphere.Application.Services;
using Atmosphere.Core.Models;

namespace Atmosphere.Services.Notifications;

public class NotificationService : INotificationService
{
    protected readonly INotificationService _wrapee;

    public NotificationService(INotificationService wrapee)
    {
        _wrapee = wrapee;
    }

    public Task Notify(Reading reading, IEnumerable<Notification> validationResults)
    {
        return Task.CompletedTask;
    }
}