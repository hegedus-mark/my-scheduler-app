using Scheduling.Application.DataTransfer.DTOs;
using SharedKernel.Persistence;

namespace Scheduling.Application.Contracts.Repositories;

public interface ITaskItemRepository : IBaseRepository<TaskItemDto> { }
