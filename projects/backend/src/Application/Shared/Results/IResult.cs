namespace Application.Shared.Results;

public interface IResult
{
    bool IsSuccess { get; }
    bool IsFailure { get; }
    ResultStatus Status { get; }
    Error? Error { get; }
}

public interface IValueResult<out T>
{
    T? Value { get; }
}

public interface ICollectionResult<out T>
{
    IEnumerable<T> Items { get; }
    int Count { get; }
}
