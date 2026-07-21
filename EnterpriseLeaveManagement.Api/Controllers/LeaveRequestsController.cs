using EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.ApplyLeave;
using EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.ApproveLeave;
using EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.CancelLeave;
using EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.RejectLeave;
using EnterpriseLeaveManagement.Application.Features.LeaveRequests.Queries.GetAllLeaveRequests;
using EnterpriseLeaveManagement.Application.Features.LeaveRequests.Queries.GetLeaveRequestById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseLeaveManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaveRequestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllLeaveRequestsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetLeaveRequestByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Apply([FromBody] ApplyLeaveCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await _mediator.Send(new CancelLeaveCommand(id));
        return NoContent();
    }

    [HttpPut("{id:guid}/approve")]
    public async Task<IActionResult> Approve(Guid id,[FromBody] ApproveLeaveCommand command)
    {
        command.LeaveRequestId = id;

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpPut("{id:guid}/reject")]
    public async Task<IActionResult> Reject(Guid id,[FromBody] RejectLeaveCommand command)
    {
        command.LeaveRequestId = id;

        await _mediator.Send(command);

        return NoContent();
    }
}