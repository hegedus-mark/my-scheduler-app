using Application.Shared.Results;

namespace Application.Shared.Messaging;

/// <summary>
///     Provides a simplified messaging interface for implementing the Mediator pattern, abstracting the communication
///     between components.
/// </summary>
/// <remarks>
///     The Mediator acts as a central hub for communication, reducing direct dependencies between components.
///     Common uses include:
///     - Sending commands that modify application state
///     - Querying data from the application
///     - Handling cross-cutting concerns like logging and validation
/// </remarks>
public interface IMediator
{
    /// <summary>
    ///     Sends a command that returns a result of type TResult.
    /// </summary>
    /// <typeparam name="TResult">The type of result expected from the command</typeparam>
    /// <param name="command">The command to execute</param>
    /// <param name="cancellationToken">Optional token to cancel the operation</param>
    /// <returns>A Result containing either the successful TResult or error details</returns>
    /// <example>
    ///     <code>
    ///      var command = new CreateUserCommand("John", "john@example.com");
    ///      var result = await mediator.SendAsync(command);
    ///  </code>
    /// </example>
    Task<Result<TResult>> SendAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///     Sends a command that doesn't return a value.
    /// </summary>
    /// <param name="command">The command to execute</param>
    /// <param name="cancellationToken">Optional token to cancel the operation</param>
    /// <returns>A Result indicating success or failure</returns>
    /// <example>
    ///     <code>
    ///    var command = new DeleteUserCommand(userId);
    ///    var result = await mediator.SendAsync(command);
    /// </code>
    /// </example>
    Task<Result> SendAsync(ICommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Sends a query to retrieve data.
    /// </summary>
    /// <typeparam name="TResult">The type of data to retrieve</typeparam>
    /// <param name="query">The query to execute</param>
    /// <param name="cancellationToken">Optional token to cancel the operation</param>
    /// <returns>The requested data of type TResult</returns>
    /// <example>
    ///     <code>
    ///     var query = new GetUserByIdQuery(userId);
    ///     var user = await mediator.SendAsync(query);
    /// </code>
    /// </example>
    Task<TResult> SendAsync<TResult>(
        IQuery<TResult> query,
        CancellationToken cancellationToken = default
    );
}
