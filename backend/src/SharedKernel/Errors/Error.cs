namespace SharedKernel.Errors;

public record Error(string Code, string Message)
{
    public static Error None = new(string.Empty, string.Empty);

    public static Error NotFound(string message)
    {
        return new Error("NotFound", message);
    }

    public static Error Validation(string message)
    {
        return new Error("Validation", message);
    }

    public static Error Conflict(string message)
    {
        return new Error("Conflict", message);
    }

    public static Error Unauthorized(string message)
    {
        return new Error("Unauthorized", message);
    }

    public static Error Forbidden(string message)
    {
        return new Error("Forbidden", message);
    }

    public static Error Internal(string message)
    {
        return new Error("Internal", message);
    }
}
