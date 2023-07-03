namespace BallastLane.Domain.Common;
public class DomainResult<TValue> : DomainResult
{
    private readonly TValue? _value;

    protected internal DomainResult(TValue? value ,bool isSuccess, Error error) 
        : base(isSuccess, error) => _value = value;

    protected internal DomainResult(TValue? value, bool isSuccess, Error[] errors)
        : base(isSuccess, errors) => _value = value;

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The operation failed. The value cannot be accesed.");

    public static implicit operator DomainResult<TValue>(TValue value) => Create(value);
}
