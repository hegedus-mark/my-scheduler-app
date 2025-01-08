namespace Application.Shared.Results;

public record Error(ResultStatus Status, string Code, string Message)
{
    public static Error None = new(ResultStatus.Ok, string.Empty, string.Empty);

    public static Error NotFound(string message)
    {
        return new Error(ResultStatus.NotFound, "NotFound", message);
    }

    public static Error Validation(string message)
    {
        return new Error(ResultStatus.BadRequest, "Validation", message);
    }

    public static Error Conflict(string message)
    {
        return new Error(ResultStatus.Conflict, "Conflict", message);
    }

    public static Error Unauthorized(string message)
    {
        return new Error(ResultStatus.Unauthorized, "Unauthorized", message);
    }

    public static Error Forbidden(string message)
    {
        return new Error(ResultStatus.Forbidden, "Forbidden", message);
    }

    public static Error Internal(string message)
    {
        return new Error(ResultStatus.InternalError, "Internal", message);
    }

    // Default to BadRequest for any unspecified error types
    public static Error New(string code, string message)
    {
        return new Error(ResultStatus.BadRequest, code, message);
    }
}
