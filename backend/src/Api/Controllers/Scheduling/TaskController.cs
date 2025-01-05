using Api.Models.Scheduling.Requests;
using Application.Scheduling.Commands;
using Application.Scheduling.DataTransfer.DTOs;
using Application.Shared.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Scheduling;

public class TaskController : BaseController
{
    private readonly IMediator _mediator;

    public TaskController(IMediator mediator)
    {
        _mediator = mediator;
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
}
