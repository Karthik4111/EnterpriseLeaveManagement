using EnterpriseLeaveManagement.Application.Features.LeaveAllocations.Commands.CreateLeaveAllocation;
using EnterpriseLeaveManagement.Application.Features.LeaveAllocations.Commands.DeleteLeaveAllocation;
using EnterpriseLeaveManagement.Application.Features.LeaveAllocations.Commands.UpdateLeaveAllocation;
using EnterpriseLeaveManagement.Application.Features.LeaveAllocations.Queries.GetAllLeaveAllocations;
using EnterpriseLeaveManagement.Application.Features.LeaveAllocations.Queries.GetLeaveAllocationById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseLeaveManagement.API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class LeaveAllocationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaveAllocationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _mediator.Send(new GetAllLeaveAllocationsQuery()));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        return Ok(await _mediator.Send(
            new GetLeaveAllocationByIdQuery { Id = id }));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateLeaveAllocationCommand command)
    {
        var id = await _mediator.Send(command);

        return Ok(id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateLeaveAllocationCommand command)
    {
        command.Id = id;

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(
            new DeleteLeaveAllocationCommand { Id = id });

        return NoContent();
    }
}