using SharedKernel.Errors;

namespace SharedKernel.Results;

public class Result<T>
{
    private Result(T? value, Error? error, ResultStatus status)
    {
        Value = value;
        Error = error;
        Status = status;
    }

    public T? Value { get; }
    public Error? Error { get; }
    public ResultStatus Status { get; }
    public bool IsSuccess => Error is null;
    public bool IsFailure => !IsSuccess;

    public static Result<T> Success(T value)
    {
        return new Result<T>(value, null, ResultStatus.Ok);
    }

    public static Result<T> Created(T value)
    {
        return new Result<T>(value, null, ResultStatus.Created);
    }

    public static Result<T> Failure(Error error)
    {
        return new Result<T>(default, error, error.Status);
    }
}

public class Result
{
    private Result(bool isSuccess, Error? error = null)
    {
        if (!isSuccess && error == null)
            throw new InvalidOperationException("A failing result must have an error");

        IsSuccess = isSuccess;
        Error = error;
        Status = error?.Status ?? ResultStatus.BadRequest;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }
    public ResultStatus Status { get; }

    // Factory methods to create Results
    public static Result Success()
    {
        return new Result(true);
    }

    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }
}
