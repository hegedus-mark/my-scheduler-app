using System.Collections;
using System.Runtime.CompilerServices;

namespace SharedKernel.Common.Guard;

/// <summary>
///     Provides guard clause methods for parameter validation.
/// </summary>
public static class Guard
{
    /// <summary>
    ///     Ensures an argument is not null.
    /// </summary>
    /// <param name="value">The value to check for null.</param>
    /// <param name="parameterName">Name of the parameter being checked.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <exception cref="ArgumentNullException">Thrown when value is null.</exception>
    public static T AgainstNull<T>(
        T? value,
        [CallerArgumentExpression("value")] string? parameterName = null,
        string? message = null
    )
        where T : class
    {
        if (value is null)
            throw new ArgumentNullException(
                parameterName,
                message ?? $"Parameter {parameterName} cannot be null"
            );

        return value;
    }

    /// <summary>
    ///     Ensures a string is not null or empty.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <param name="parameterName">Name of the parameter being checked.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <exception cref="ArgumentException">Thrown when string is null or empty.</exception>
    public static string AgainstNullOrEmpty(
        string? value,
        [CallerArgumentExpression("value")] string? parameterName = null,
        string? message = null
    )
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException(
                message ?? $"Parameter {parameterName} cannot be null or empty",
                parameterName
            );

        return value;
    }

    /// <summary>
    ///     Ensures a string is not null, empty, or whitespace.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <param name="parameterName">Name of the parameter being checked.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <exception cref="ArgumentException">Thrown when string is null, empty, or whitespace.</exception>
    public static string AgainstNullOrWhiteSpace(
        string? value,
        [CallerArgumentExpression("value")] string? parameterName = null,
        string? message = null
    )
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                message ?? $"Parameter {parameterName} cannot be null, empty, or whitespace",
                parameterName
            );

        return value;
    }

    /// <summary>
    ///     Ensures a collection is not null or empty.
    /// </summary>
    /// <param name="value">The collection to check.</param>
    /// <param name="parameterName">Name of the parameter being checked.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <exception cref="ArgumentException">Thrown when collection is null or empty.</exception>
    public static ICollection AgainstNullOrEmpty(
        ICollection? value,
        [CallerArgumentExpression("value")] string? parameterName = null,
        string? message = null
    )
    {
        AgainstNull(value, parameterName);

        if (value!.Cast<object>().Any())
            throw new ArgumentException(
                message ?? $"Parameter {parameterName} cannot be empty",
                parameterName
            );

        return value!;
    }

    /// <summary>
    ///     Ensures a value is greater than a specified number.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="minimum">The minimum allowed value (exclusive).</param>
    /// <param name="parameterName">Name of the parameter being checked.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when value is less than or equal to minimum.</exception>
    public static T AgainstLessThanOrEqual<T>(
        T value,
        T minimum,
        [CallerArgumentExpression("value")] string? parameterName = null,
        string? message = null
    )
        where T : IComparable<T>
    {
        if (value.CompareTo(minimum) <= 0)
            throw new ArgumentOutOfRangeException(
                parameterName,
                message ?? $"Parameter {parameterName} must be greater than {minimum}"
            );

        return value;
    }

    /// <summary>
    ///     Ensures a value falls within a specified range.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="minimum">The minimum allowed value (inclusive).</param>
    /// <param name="maximum">The maximum allowed value (inclusive).</param>
    /// <param name="parameterName">Name of the parameter being checked.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when value is outside the specified range.</exception>
    public static T AgainstOutOfRange<T>(
        T value,
        T minimum,
        T maximum,
        [CallerArgumentExpression("value")] string? parameterName = null,
        string? message = null
    )
        where T : IComparable<T>
    {
        if (value.CompareTo(minimum) < 0 || value.CompareTo(maximum) > 0)
            throw new ArgumentOutOfRangeException(
                parameterName,
                message ?? $"Parameter {parameterName} must be between {minimum} and {maximum}"
            );

        return value;
    }

    /// <summary>
    ///     Ensures a date is in the future.
    /// </summary>
    /// <param name="value">The date to check.</param>
    /// <param name="parameterName">Name of the parameter being checked.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <exception cref="ArgumentException">Thrown when date is not in the future.</exception>
    public static DateTime AgainstPastDate(
        DateTime value,
        [CallerArgumentExpression("value")] string? parameterName = null,
        string? message = null
    )
    {
        if (value <= DateTime.Now)
            throw new ArgumentException(
                message ?? $"Parameter {parameterName} must be a future date",
                parameterName
            );

        return value;
    }

    /// <summary>
    ///     Ensures a Guid is not empty.
    /// </summary>
    /// <param name="value">The Guid to check.</param>
    /// <param name="parameterName">Name of the parameter being checked.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <exception cref="ArgumentException">Thrown when Guid is empty.</exception>
    public static Guid AgainstEmpty(
        Guid value,
        [CallerArgumentExpression("value")] string? parameterName = null,
        string? message = null
    )
    {
        if (value == Guid.Empty)
            throw new ArgumentException(
                message ?? $"Parameter {parameterName} cannot be an empty GUID",
                parameterName
            );

        return value;
    }

    /// <summary>
    ///     Ensures a condition is true.
    /// </summary>
    /// <param name="condition">The condition to check.</param>
    /// <param name="message">The error message if the condition is false.</param>
    /// <param name="parameterName">Name of the parameter being checked.</param>
    /// <exception cref="ArgumentException">Thrown when condition is false.</exception>
    public static void AgainstFalseCondition(
        bool condition,
        string message,
        [CallerArgumentExpression("condition")] string? parameterName = null
    )
    {
        if (!condition)
            throw new ArgumentException(message, parameterName);
    }
}
