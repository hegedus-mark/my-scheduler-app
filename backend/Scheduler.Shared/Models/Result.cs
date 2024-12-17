using Scheduler.Shared.Enums;

namespace Scheduler.Shared.Models;

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
