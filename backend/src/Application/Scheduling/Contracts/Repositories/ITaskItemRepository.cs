using Domain.Scheduling.Models;
using SharedKernel.Persistence;

namespace Application.Scheduling.Contracts.Repositories;

public interface ITaskItemRepository : IBaseRepository<TaskItem> { }
