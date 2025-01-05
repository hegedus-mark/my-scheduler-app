using Api.Models.Scheduling.Requests;
using Application.Scheduling.Commands;
using Application.Scheduling.DataTransfer.DTOs;
using Application.Scheduling.Queries;
using Application.Shared.Messaging;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Results;

namespace Api.Controllers.Scheduling;

public class TaskController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public TaskController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<TaskItemDto>> UpdateTask(
        Guid id,
        [FromBody] UpdateTaskRequest request
    )
    {
        var command = new UpdateTaskCommand(
            id,
            request.Name,
            request.DueDate,
            request.Duration,
            request.Priority
        );

        var result = await _mediator.SendAsync(command);
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<TaskItemDto>> CreateTask([FromBody] CreateTaskRequest request)
    {
        var command = _mapper.Map<CreateTaskCommand>(request);
        var result = await _mediator.SendAsync(command);
        return Ok(result.Value);
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetAllTasks()
    {
        var query = new GetAllTasksQuery();
        var result = await _mediator.SendAsync(query);

        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Result>> DeleteTask(Guid id)
    {
        var command = new DeleteTaskCommand(id);

        var result = await _mediator.SendAsync(command);
        return Ok(result);
    }
}
