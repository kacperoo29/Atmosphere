using System.Linq.Expressions;

namespace Atmosphere.Application.Extensions;

public static class ExpressionExtensions
{
    public static string ToStringReadable(this Expression expression)
    {
        return expression.ToString().Replace("AndAlso", "&&").Replace("OrElse", "||");
    }
}
