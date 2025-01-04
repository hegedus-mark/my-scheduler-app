namespace Domain.Scheduling.Exceptions;

public class TaskStateTransitionException : Exception
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
