namespace Atmoshpere.Application.Exceptions;

public class AuthException : Exception
{
    public AuthException(string message)
        : base(message)
    {
        
    }
}