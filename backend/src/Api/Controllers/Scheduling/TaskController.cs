using Api.Infrastructure.Attributes;
using Api.Models.Scheduling.Requests;
using Application.Scheduling.Commands;
using Application.Scheduling.DataTransfer.DTOs;
using Application.Scheduling.Queries;
using Application.Shared.Messaging;
using Application.Shared.Results;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

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
    [ExpectedResults(ResultStatus.Ok, ResultStatus.NotFound)]
    [ProducesResponseType(typeof(TaskItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Result<TaskItemDto>>> UpdateTask(
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

        return await _mediator.SendAsync(command);
    }

    [HttpPost]
    [ExpectedResults(ResultStatus.Ok)]
    [ProducesResponseType(typeof(TaskItemDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<Result<TaskItemDto>>> CreateTask(
        [FromBody] CreateTaskRequest request
    )
    {
        var command = _mapper.Map<CreateTaskCommand>(request);
        return await _mediator.SendAsync(command);
    }

    [HttpGet("all")]
    [ExpectedResults(ResultStatus.Ok)]
    [ProducesResponseType(typeof(TaskItemDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CollectionResult<TaskItemDto>>> GetAllTasks()
    {
        var query = new GetAllTasksQuery();
        return await _mediator.SendAsync(query);
    }

    [HttpDelete("{id}")]
    [ExpectedResults(ResultStatus.Ok, ResultStatus.NotFound)]
    [ProducesResponseType(typeof(TaskItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Result>> DeleteTask(Guid id)
    {
        var command = new DeleteTaskCommand(id);

        return await _mediator.SendAsync(command);
    }
}
