namespace ProductMaintenance.Exceptions;

public abstract class ApiException : Exception
{
    protected ApiException(string? message)
        : base(message)
    {
    }
}
