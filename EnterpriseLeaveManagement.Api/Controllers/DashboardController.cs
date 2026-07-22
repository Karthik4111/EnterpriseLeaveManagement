using EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetAdminDashboard;
using EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetEmployeeDashboard;
using EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetManagerDashboard;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseLeaveManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAdminDashboard()
    {
        var result = await _mediator.Send(new GetAdminDashboardQuery());
        return Ok(result);
    }

    [HttpGet("employee")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> GetEmployeeDashboard()
    {
        var result = await _mediator.Send(new GetEmployeeDashboardQuery());

        return Ok(result);
    }

    [HttpGet("manager")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetManagerDashboard()
    {
        var result = await _mediator.Send(new GetManagerDashboardQuery());
        return Ok(result);
    }
}