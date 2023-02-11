using System.ComponentModel.DataAnnotations;
using Atmosphere.Core.Models;

namespace Atmosphere.Core.Validation;

public interface IReadingValidator
{
    Task<IEnumerable<Notification>> Validate(Reading reading);
}