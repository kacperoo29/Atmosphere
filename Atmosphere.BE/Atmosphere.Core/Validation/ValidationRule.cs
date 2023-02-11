using System.Linq.Expressions;
using Atmosphere.Core.Enums;
using Atmosphere.Core.Models;

namespace Atmosphere.Core.Validation;

public class ValidationRule 
{
    public string Message { get; init; }
    public Severity Severity { get; init; }
    public Expression<Func<Reading, bool>> Condition { get; init; }
}
