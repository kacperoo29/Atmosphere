using System.ComponentModel.DataAnnotations;
using Atmosphere.Core.Models;

namespace Atmosphere.Application.Services;

public interface INotificationService
{
    Task Notify(Reading reading, List<ValidationResult> validationResults);
}