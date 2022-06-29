namespace Atmosphere.Core.Services;

using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Atmosphere.Core.Models;

public abstract class IReadingValidationService
{
    public async IAsyncEnumerable<ValidationResult> RunChecks(Reading reading)
    {
        yield return await CheckIfInRange(reading);
    }

    protected abstract Task<ValidationResult> CheckIfInRange(Reading reading);
    protected abstract Task<ValidationResult> CompareToPrevious(Reading reading);
}
