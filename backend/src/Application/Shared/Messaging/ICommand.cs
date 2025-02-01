namespace Application.Shared.Messaging;

/// <summary>
///     Marker interface for commands that return a result.
///     Commands represent intentions to change the system state.
/// </summary>
/// <typeparam name="TResult">The type of result returned by the command</typeparam>
/// <remarks>
///     Commands should:
///     - Be named in imperative form (e.g., CreateUser, UpdateOrder)
///     - Modify system state
///     - Return results indicating success/failure and any created/updated data
/// </remarks>
public interface ICommand<TResult> { }

/// <summary>
///     Marker interface for commands that don't return a result.
/// </summary>
/// <remarks>
///     Use this interface for commands that only need to indicate success/failure
///     without returning any data.
/// </remarks>
public interface ICommand { }
