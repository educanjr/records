
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BallastLane.Domain.Common;
public class DomainResult
{
	public bool IsSuccess { get; }
	public bool IsFailure => !IsSuccess;
	public Error[] Errors { get; }

    public Error[] ValidErrors => Errors.Where(i => i != Error.None).ToArray();

	protected internal DomainResult(bool isSuccess, Error error)
	{
		if (isSuccess && error != Error.None)
		{
			throw new InvalidOperationException();
		}

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }

		IsSuccess = isSuccess;
		Errors = new[] { error };
    }

	protected internal DomainResult(bool isSuccess, Error[] errors) 
	{
        IsSuccess = isSuccess;
		Errors = errors;
    }

	public static DomainResult Success() => new(true, Error.None);

    public static DomainResult Failure(Error error) => new(false, error);

    public static DomainResult Failure(Error[] errors) => new(false, errors);

    public static DomainResult<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static DomainResult<TValue> Failure<TValue>(Error error) => new(default, false, error);

    public static DomainResult<TValue> Failure<TValue>(Error[] errors) => new(default, false, errors);

    public static DomainResult<TValue> Create<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

    public static DomainResult<T> Ensure<T>(T value, Func<T, bool> predicate, Error error)
    {
        return predicate(value) ?
            Success(value) :
            Failure<T>(error);
    }

    public static DomainResult<T> Ensure<T>(
        T value,
        params (Func<T, bool> predicate, Error error)[] functions)
    {
        var results = new List<DomainResult<T>>();
        foreach ((Func<T, bool> predicate, Error error) in functions)
        {
            results.Add(Ensure(value, predicate, error));
        }

        return Combine(results.ToArray());
    }

    public static DomainResult<T> Combine<T>(params DomainResult<T>[] results)
    {
        if (results.Any(r => r.IsFailure))
        {
            return Failure<T>(
                results
                    .SelectMany(r => r.Errors)
                    .Where(e => e != Error.None)
                    .Distinct()
                    .ToArray());
        }

        return Success(results[0].Value);
    }

    public static DomainResult<(T1, T2)> Combine<T1, T2>(DomainResult<T1> result1, DomainResult<T2> result2)
    {
        if (result1.IsFailure)
        {
            return Failure<(T1, T2)>(result1.Errors);
        }

        if (result2.IsFailure)
        {
            return Failure<(T1, T2)>(result2.Errors);
        }

        return Success((result1.Value, result2.Value));
    }
}

