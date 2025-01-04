using SharedKernel.Errors;

namespace SharedKernel.Results;

//TODO: probably shouldn't couple it with APi request status codes. let the api manage that

public class Result<T>
{
    private readonly List<Error> _errors = new();

    private Result(T? value, List<Error> errors, ResultStatus status)
    {
        Value = value;
        _errors = errors;
        Status = status;
    }

    public T? Value { get; }
    public IReadOnlyList<Error> Errors => _errors.AsReadOnly();
    public ResultStatus Status { get; }
    public bool IsSuccess => !_errors.Any();
    public bool IsFailure => !IsSuccess;

    public static Result<T> Success(T value)
    {
        return new Result<T>(value, new List<Error>(), ResultStatus.Ok);
    }

    public static Result<T> Created(T value)
    {
        return new Result<T>(value, new List<Error>(), ResultStatus.Created);
    }

    public static Result<T> Failure(Error error, ResultStatus status = ResultStatus.BadRequest)
    {
        return new Result<T>(default, new List<Error> { error }, status);
    }

    public static Result<T> Failure(
        List<Error> errors,
        ResultStatus status = ResultStatus.BadRequest
    )
    {
        return new Result<T>(default, errors, status);
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
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    // Factory methods to create Results
    public static Result Success()
    {
        return new Result(true);
    }

    public static Result Failure(string message)
    {
        return new Result(false, new Error("StateTransition.Failed", message));
    }

    public Result OnSuccess(Action action)
    {
        if (IsSuccess)
            action();
        return this;
    }

    public Result OnFailure(Action<Error> action)
    {
        if (IsFailure)
            action(Error!);
        return this;
    }
}
