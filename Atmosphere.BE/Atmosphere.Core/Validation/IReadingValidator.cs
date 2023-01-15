using System.ComponentModel.DataAnnotations;
using Atmosphere.Core.Models;

namespace Atmosphere.Core.Validation;

public interface IReadingValidator
{
    Task<IEnumerable<ValidationResult>> Validate(Reading reading);
}