namespace StageSim.Domain.Exceptions;

public interface IDomainException
{
    int StatusCode { get; }
}
