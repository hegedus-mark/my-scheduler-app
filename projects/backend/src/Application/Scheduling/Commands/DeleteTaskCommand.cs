using Application.Shared.Messaging;

namespace Application.Scheduling.Commands;

public record DeleteTaskCommand(Guid TaskId) : ICommand { }
