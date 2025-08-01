namespace Vinder.IdentityProvider.Common.Results;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error)
    {
        if (error == null || error == Error.None)
            throw new ArgumentException("Invalid error for failure.", nameof(error));

        return new Result(false, error);
    }
}
