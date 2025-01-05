using Application.Scheduling.DataTransfer.DTOs;
using Application.Shared.Messaging;

namespace Application.Scheduling.Queries;

public class GetAllTasksQuery : IQuery<IReadOnlyCollection<TaskItemDto>> { }
