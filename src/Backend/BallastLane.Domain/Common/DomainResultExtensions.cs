
namespace BallastLane.Domain.Common;

public static class DomainResultExtensions
{
    public static DomainResult<T> Ensure<T>(
        this DomainResult<T> result,
        Func<T, bool> predicate,
        Error error)
    {
        if(result.IsFailure)
        {
            return result;
        }

        return predicate(result.Value)
            ? result
            : DomainResult.Failure<T>(error);
    }

    public static DomainResult<TOut> Map<TIn, TOut>(
        this DomainResult<TIn> result,
        Func<TIn, TOut> predicate)
    {
        return result.IsSuccess
            ? DomainResult.Success(predicate(result.Value))
            : DomainResult.Failure<TOut>(result.Errors);
    }

    public static async Task<DomainResult> Bind<TIn>(
        this DomainResult<TIn> result,
        Func<TIn, Task<DomainResult>> predicate)
    {
        if(result.IsFailure)
        {
            return DomainResult.Failure(result.Errors);
        }

        return await predicate(result.Value);
    }

    public static async Task<DomainResult<TOut>> Bind<TIn, TOut>(
        this DomainResult<TIn> result,
        Func<TIn, Task<DomainResult<TOut>>> predicate)
    {
        if (result.IsFailure)
        {
            return DomainResult.Failure<TOut>(result.Errors);
        }

        return await predicate(result.Value);
    }
}
