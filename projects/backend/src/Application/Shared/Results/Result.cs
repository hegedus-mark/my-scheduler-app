namespace Application.Shared.Results;

public abstract class ResultBase : IResult
{
    protected ResultBase(ResultStatus status)
    {
        Status = status;
        IsSuccess = true;
    }

    protected ResultBase(Error error)
    {
        Error = error;
        Status = error.Status;
        IsSuccess = false;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public ResultStatus Status { get; }
    public Error? Error { get; }
}

public sealed class Result : ResultBase
{
    private Result(ResultStatus status)
        : base(status) { }

    private Result(Error error)
        : base(error) { }

    public static Result Success(ResultStatus status = ResultStatus.Ok)
    {
        return new Result(status);
    }

    public static Result Failure(Error error)
    {
        return new Result(error);
    }
}

public sealed class Result<T> : ResultBase, IValueResult<T>
{
    private Result(T value, ResultStatus status)
        : base(status)
    {
        Value = value;
    }

    private Result(Error error)
        : base(error)
    {
        Value = default;
    }

    public T? Value { get; }

    public static Result<T> Success(T value, ResultStatus status = ResultStatus.Ok)
    {
        return new Result<T>(value, status);
    }

    public static Result<T> Failure(Error error)
    {
        return new Result<T>(error);
    }
}

public sealed class CollectionResult<T> : ResultBase, ICollectionResult<T>
{
    private CollectionResult(IEnumerable<T> items, ResultStatus status)
        : base(status)
    {
        Items = items;
    }

    private CollectionResult(Error error)
        : base(error)
    {
        Items = [];
    }

    public bool IsEmpty => !Items.Any();
    public IEnumerable<T> Items { get; }
    public int Count => Items.Count();

    public static CollectionResult<T> Success(
        IEnumerable<T> items,
        ResultStatus status = ResultStatus.Ok
    )
    {
        return new CollectionResult<T>(items, status);
    }

    public static CollectionResult<T> Failure(Error error)
    {
        return new CollectionResult<T>(error);
    }
}
