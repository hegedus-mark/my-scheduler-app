# Messaging Pattern Documentation

## Overview

Our application uses the Mediator pattern combined with CQRS to handle all business operations. This document explains how these patterns are implemented and how to use them effectively.

## Key Components

### Commands

Commands represent intentions to change the system state. They should be named in the imperative form.

```csharp
// Example command
public record CreateTaskCommand(
    string Name,
    DateTime DueDate,
    TimeSpan Duration,
    PriorityLevel Priority
) : ICommand<TaskItemDto>;
```

### Queries

Queries request data from the system without making any changes.

```csharp
// Example query
public class GetAllTasksQuery : ICollectionQuery<TaskItemDto> { }
```

### Handlers

Each command or query has exactly one handler that contains the business logic.

```csharp
// Example handler
public class CreateTaskCommandHandler : ICommandHandler<CreateTaskCommand, TaskItemDto>
{
    private readonly ISchedulingUnitOfWork _unitOfWork;

    public CreateTaskCommandHandler(ISchedulingUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TaskItemDto>> HandleAsync(
        CreateTaskCommand command,
        CancellationToken cancellationToken = default)
    {
        var task = TaskItem.Create(
            command.Name,
            command.DueDate,
            command.Duration,
            command.Priority
        );

        await _unitOfWork.TaskItems.AddAsync(task);
        await _unitOfWork.SaveChangesAsync();

        return Result<TaskItemDto>.Success(task.ToDto());
    }
}
```

## Best Practices

1. **Command Naming**

   - Use imperative form (Create, Update, Delete)
   - Be specific about the action (UpdateTaskDueDate vs Update)

2. **Query Naming**

   - Use descriptive forms (GetTaskById, ListActiveTasks)
   - Include filtering criteria in the name when applicable

3. **Handler Responsibilities**

   - Keep handlers focused on a single operation
   - Use domain services for complex business logic
   - Maintain transactional boundaries

4. **Results**
   - Commands return `Result<T>` or `Result`
   - Queries return direct data or `CollectionResult<T>`

## Usage Examples

### In Controllers

```csharp
[HttpPost]
public async Task<ActionResult<Result<TaskItemDto>>> CreateTask(
    [FromBody] CreateTaskRequest request)
{
    var command = _mapper.Map<CreateTaskCommand>(request);
    return await _mediator.SendAsync(command);
}
```

### In Application Services

```csharp
public async Task<UserDto> GetCurrentUser(int userId)
{
    var query = new GetUserByIdQuery(userId);
    return await _mediator.SendAsync(query);
}
```

## Common Patterns and Solutions

### Handling Validation

- Use FluentValidation in command/query validators
- Return `Result.Failure` with validation errors

### Error Handling

- Use custom Result types for different scenarios
- Include meaningful error messages and codes
