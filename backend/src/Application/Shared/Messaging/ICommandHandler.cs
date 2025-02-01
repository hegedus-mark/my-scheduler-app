using Application.Shared.Results;

namespace Application.Shared.Messaging;

/// <summary>
///     Defines a handler for commands that return results.
/// </summary>
/// <typeparam name="TCommand">The type of command to handle</typeparam>
/// <typeparam name="TResult">The type of result returned</typeparam>
/// <remarks>
///     Handlers contain the business logic for processing commands.
///     Each command should have exactly one handler.
/// </remarks>
public interface ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    /// <summary>
    ///     Handles the specified command and returns a result.
    /// </summary>
    /// <param name="command">The command to handle</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>A Result containing the command's outcome</returns>
    Task<Result<TResult>> HandleAsync(
        TCommand command,
        CancellationToken cancellationToken = default
    );
}

/// <summary>
///     Defines a handler for commands that don't return results.
/// </summary>
/// <typeparam name="TCommand">The type of command to handle</typeparam>
public interface ICommandHandler<TCommand>
    where TCommand : ICommand
{
    /// <summary>
    ///     Handles the specified command.
    /// </summary>
    /// <param name="command">The command to handle</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>A Result indicating success or failure</returns>
    Task<Result> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}
