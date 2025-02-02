using Api.Models.Scheduling.Requests;
using Application.Scheduling.Commands;
using Application.Scheduling.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Scheduling;

public class SchedulingController : BaseController
{
    private readonly IMapper _mapper;
    private readonly ISchedulingService _schedulingService;

    public SchedulingController(IMapper mapper, ISchedulingService schedulingService)
    {
        _mapper = mapper;
        _schedulingService = schedulingService;
    }

    [HttpPost("schedule")]
    public async Task<IActionResult> ScheduleTasks([FromBody] ScheduleTasksRequest request)
    {
        var command = _mapper.Map<ScheduleTasksCommand>(request);
        var result = await _schedulingService.ScheduleTaskAsync(command);
        return Ok(result);
    }
}
