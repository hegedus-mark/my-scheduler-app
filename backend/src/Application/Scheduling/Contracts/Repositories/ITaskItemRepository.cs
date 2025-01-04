using Scheduling.Domain.Models;
using SharedKernel.Persistence;

namespace Application.Scheduling.Contracts.Repositories;

public interface ITaskItemRepository : IBaseRepository<TaskItem> { }
