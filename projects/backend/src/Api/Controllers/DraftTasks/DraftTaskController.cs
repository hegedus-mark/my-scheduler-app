using Api.Controllers.DraftTasks.Model;
using Application.DraftTasks;
using Application.DraftTasks.CreateDraftTask;
using Application.DraftTasks.DeleteDraftTask;
using Application.DraftTasks.GetAllDraftTasks;
using Application.DraftTasks.UpdateDraftTask;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.DraftTasks;

public class DraftTaskController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IDeleteDraftTaskService _deleteDraftTaskService;
    private readonly ICreateDraftTaskService _createDraftTaskService;
    private readonly IUpdateDraftTaskService _updateDraftTaskService;
    private readonly IGetAllDraftTasksService _getAllDraftTasksService;

    public DraftTaskController(IMapper mapper, ICreateDraftTaskService createDraftTaskService,
        IDeleteDraftTaskService deleteDraftTaskService, IUpdateDraftTaskService updateDraftTaskService,
        IGetAllDraftTasksService getAllDraftTasksService)
    {
        _mapper = mapper;
        _createDraftTaskService = createDraftTaskService;
        _deleteDraftTaskService = deleteDraftTaskService;
        _updateDraftTaskService = updateDraftTaskService;
        _getAllDraftTasksService = getAllDraftTasksService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateDraftTaskResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<CreateDraftTaskResponse>> CreateDraftTask(
        [FromBody] CreateDraftTaskRequest request
    )
    {
        var command = _mapper.Map<CreateDraftTaskCommand>(request);
        var id = await _createDraftTaskService.CreateDraftTask(command);

        var response = new CreateDraftTaskResponse { Id = id };
        return Ok(response);
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateDraftTask(Guid id, UpdateDraftTaskRequest request)
    {
        //Id does not exist in the request, so we have to supply id to the command from the route parameter seperately
        //This is one way of doing that
        var command = _mapper.Map<UpdateDraftTaskCommand>(
            request,
            opt => opt.Items["Id"] = id);

        await _updateDraftTaskService.UpdateDraftTaskAsync(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteDraftTask(Guid id)
    {
        var command = new DeleteDraftTaskCommand(id);

        await _deleteDraftTaskService.DeleteDraftTaskAsync(command);

        return NoContent();
    }

    [HttpGet("all")]
    [ProducesResponseType(typeof(List<DraftTask>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DraftTask>>> GetAll()
    {
        var tasks = await _getAllDraftTasksService.GetAllDraftTasksAsync();
        return Ok(tasks);
    }
}