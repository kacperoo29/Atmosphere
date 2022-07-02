namespace Atmosphere.Core.Rules;

using System.ComponentModel.DataAnnotations;

public interface IValidationRule<T>
{
    Task<ValidationResult> ValidateRange(T reading);
    Task<ValidationResult> ValidateWithPrevious(T reading, T previous);
}
