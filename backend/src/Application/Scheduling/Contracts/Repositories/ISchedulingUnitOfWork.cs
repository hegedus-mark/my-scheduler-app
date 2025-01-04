using SharedKernel.Persistence;

namespace Application.Scheduling.Contracts.Repositories;

public interface ISchedulingUnitOfWork : IBaseUnitOfWork
{
    ITaskItemRepository TaskItems { get; }
}
