using EnterpriseLeaveManagement.Application.Common.Models;
using EnterpriseLeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeaveType;
using EnterpriseLeaveManagement.Application.Features.LeaveTypes.Commands.DeleteLeaveType;
using EnterpriseLeaveManagement.Application.Features.LeaveTypes.Commands.UpdateLeaveType;
using EnterpriseLeaveManagement.Application.Features.LeaveTypes.DTOs;
using EnterpriseLeaveManagement.Application.Features.LeaveTypes.Queries.GetAllLeaveTypes;
using EnterpriseLeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveTypeById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseLeaveManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaveTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<LeaveTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<LeaveTypeDto>>> GetAll([FromQuery] GetAllLeaveTypesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }


    [Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(LeaveTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LeaveTypeDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetLeaveTypeByIdQuery(id));
        return Ok(result);
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Guid>> Create(CreateLeaveTypeCommand command)
    {
        var id = await _mediator.Send(command);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            id);
    }



    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, UpdateLeaveTypeCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        await _mediator.Send(command);

        return NoContent();
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteLeaveTypeCommand { Id = id });

        return NoContent();
    }
}