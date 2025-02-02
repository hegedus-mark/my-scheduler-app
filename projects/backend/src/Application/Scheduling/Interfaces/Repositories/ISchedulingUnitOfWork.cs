using Application.Shared.Contracts;

namespace Application.Scheduling.Interfaces.Repositories;

public interface ISchedulingUnitOfWork : IBaseUnitOfWork
{
    ITaskItemRepository TaskItems { get; }
}
