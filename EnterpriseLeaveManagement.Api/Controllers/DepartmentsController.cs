using EnterpriseLeaveManagement.Application.Common.Models;
using EnterpriseLeaveManagement.Application.Features.Departments.Commands.CreateDepartment;
using EnterpriseLeaveManagement.Application.Features.Departments.Commands.DeleteDepartment;
using EnterpriseLeaveManagement.Application.Features.Departments.Commands.UpdateDepartment;
using EnterpriseLeaveManagement.Application.Features.Departments.Common;
using EnterpriseLeaveManagement.Application.Features.Departments.Queries.GetAllDepartments;
using EnterpriseLeaveManagement.Application.Features.Departments.Queries.GetDepartmentById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseLeaveManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DepartmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(typeof(CreateDepartmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CreateDepartmentResponse>> Create(CreateDepartmentCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }


    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<DepartmentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<DepartmentDto>>> GetAll([FromQuery] GetAllDepartmentsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DepartmentDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetDepartmentByIdQuery(id));

        if (result is null)
            return NotFound();

        return Ok(result);
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UpdateDepartmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UpdateDepartmentResponse>> Update(Guid id,UpdateDepartmentCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("Route Id and Request Id do not match.");
        }

        var response = await _mediator.Send(command);

        return Ok(response);
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(DeleteDepartmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeleteDepartmentResponse>> Delete(Guid id)
    {
        var response = await _mediator.Send(new DeleteDepartmentCommand
        {
            Id = id
        });

        return Ok(response);
    }

}