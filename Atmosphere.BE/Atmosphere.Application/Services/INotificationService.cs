namespace Atmosphere.Application.Services;

using System.ComponentModel.DataAnnotations;

using Atmosphere.Core.Models;

public interface INotificationService
{
    Task Notify(Reading reading, List<ValidationResult> validationResults);
}