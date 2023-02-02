namespace Atmosphere.Services.Exceptions;

public class InvalidRuleException : Exception
{
    public InvalidRuleException(string message) : base(message) { }
}
