using Application.Shared.Contracts;

namespace Application.Scheduling.Contracts.Repositories;

public interface ISchedulingUnitOfWork : IBaseUnitOfWork
{
    ITaskItemRepository TaskItems { get; }
}
