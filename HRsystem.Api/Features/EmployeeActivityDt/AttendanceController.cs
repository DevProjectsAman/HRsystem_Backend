using HRsystem.Api.Features.Attendance;
using HRsystem.Api.Features.EmployeeActivityDt.EmployeePunch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EmployeeWorkflowController : ControllerBase
{
    private readonly IMediator _mediator;
    public EmployeeWorkflowController(IMediator mediator) => _mediator = mediator;

    // ------------------ PUNCH ------------------
    [HttpPost("punch/in")]
    public async Task<ActionResult<EmployeeAttendanceDto>> PunchIn([FromBody] PunchInCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("punch/out")]
    public async Task<ActionResult<EmployeeAttendanceDto>> PunchOut([FromBody] PunchOutCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
