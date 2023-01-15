using System.Linq.Expressions;
using Atmosphere.Core.Models;

namespace Atmosphere.Core.Validation;

public class ValidationRule 
{
    public string Message { get; set; }
    public decimal Value { get; set; }
    public Expression<Func<Reading, bool>> Condition { get; set; }
}
