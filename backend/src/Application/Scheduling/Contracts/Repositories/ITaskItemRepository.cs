using Application.Scheduling.DataTransfer.DTOs;
using SharedKernel.Persistence;

namespace Application.Scheduling.Contracts.Repositories;

public interface ITaskItemRepository : IBaseRepository<TaskItemDto> { }
