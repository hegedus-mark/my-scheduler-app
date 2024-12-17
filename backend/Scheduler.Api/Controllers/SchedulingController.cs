using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchedularPrototype.Models.Requests;
using Scheduler.Application.Commands.ScheduleTasks;
using Scheduler.Application.Interfaces.Services;
using Scheduler.Application.Services;

namespace SchedularPrototype.Controllers;

public class SchedulingController : BaseController
{
    private readonly IMapper _mapper;
    private readonly ISchedulingService _schedulingService;

    public SchedulingController(SchedulingService schedulingService, IMapper mapper)
    {
        _schedulingService = schedulingService;
        _mapper = mapper;
    }

    [HttpPost("schedule")]
    public async Task<IActionResult> ScheduleTasks([FromBody] ScheduleTasksRequest request)
    {
        var command = _mapper.Map<ScheduleTasksCommand>(request);
        var result = await _schedulingService.ScheduleTasksAsync(command);
        return Ok(result);
    }
}
