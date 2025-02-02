using Application.Shared.Contracts;
using Domain.Scheduling.Models;

namespace Application.Scheduling.Interfaces.Repositories;

public interface ITaskItemRepository : IBaseRepository<TaskItem>, IFastDeleteRepository { }
