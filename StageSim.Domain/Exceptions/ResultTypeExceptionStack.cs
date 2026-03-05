namespace StageSim.Domain.Exceptions;

public class BadRequestException : Exception, IDomainException
{
    public int StatusCode => 400;

    public BadRequestException() : base("Bad request!") { }
    public BadRequestException(string message) : base(message) { }
    public BadRequestException(string message, Exception innerException) : base(message, innerException) { }
}
