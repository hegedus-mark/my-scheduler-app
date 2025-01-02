namespace Scheduling.Application.Contracts.Repositories;

public interface ISchedulingRepositoryContainer
{
    ITaskItemRepository TaskItems { get; }
}
