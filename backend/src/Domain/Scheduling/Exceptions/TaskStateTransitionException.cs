using Domain.Shared.Exceptions;

namespace Domain.Scheduling.Exceptions;

/// <summary>
///     Exception thrown when an invalid state transition is attempted on a task.
///     This exception is used to maintain the integrity of the task state machine.
/// </summary>
/// <remarks>
///     This exception is typically thrown when:
///     - An operation is not allowed in the current state
///     - A state transition violates the defined state machine rules
///     - An action is attempted on a task in an incompatible state
/// </remarks>
public class TaskStateTransitionException : DomainException
{
    public TaskStateTransitionException(string state, string attemptedAction)
        : base($"Invalid transition in state {state}: {attemptedAction}")
    {
        State = state;
        AttemptedAction = attemptedAction;
    }

    public string State { get; }
    public string AttemptedAction { get; }
}
