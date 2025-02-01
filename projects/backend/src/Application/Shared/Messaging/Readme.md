# Messaging Components Quick Reference

This folder contains the core messaging components used throughout the application. For detailed documentation, see [Messaging Pattern Documentation](/docs/technical/messaging.md).

## Key Interfaces

- `IMediator`: Central hub for sending commands and queries
- `ICommand<TResult>`: Marker interface for commands that return results
- `IQuery<TResult>`: Marker interface for queries
- `ICommandHandler<TCommand, TResult>`: Interface for command handlers
- `IQueryHandler<TQuery, TResult>`: Interface for query handlers

## Quick Start

1. **Creating a Command**

```csharp
public record CreateTaskCommand(string Name) : ICommand<TaskDto>;
```

2. **Creating a Handler**

```csharp
public class CreateTaskHandler : ICommandHandler<CreateTaskCommand, TaskDto>
{
    public async Task<Result<TaskDto>> HandleAsync(
        CreateTaskCommand command,
        CancellationToken cancellationToken)
    {
        // Implementation
    }
}
```

3. **Using the Mediator**

```csharp
var result = await _mediator.SendAsync(new CreateTaskCommand("New Task"));
```
