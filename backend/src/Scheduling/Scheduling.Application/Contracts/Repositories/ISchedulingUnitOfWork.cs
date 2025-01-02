using SharedKernel.Persistence;

namespace Scheduling.Application.Contracts.Repositories;

public interface ISchedulingUnitOfWork : IBaseUnitOfWork
{
    ITaskItemRepository TaskItems { get; }
}
