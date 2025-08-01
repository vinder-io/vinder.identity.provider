namespace Vinder.IdentityProvider.Common.Results;

public class Result<TData>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public Error Error { get; }
    public TData? Data { get; }

    protected Result(bool isSuccess, TData? data, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
        Data = data;
    }

    public static Result<TData> Success(TData data) =>
        new(true, data, Error.None);

    public static Result<TData> Failure(Error error)
    {
        if (error == null || error == Error.None)
            throw new ArgumentException("Invalid error for failure.", nameof(error));

        return new Result<TData>(false, default, error);
    }
}
