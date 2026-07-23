using EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetAdminDashboard;
using EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetEmployeeDashboard;
using EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetManagerDashboard;
using EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetDashboardSummary;
using EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetLeaveStatusStatistics;
using EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetMonthlyLeaveTrend;
using EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetDepartmentStatistics;
using EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetLeaveTypeStatistics;
using EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetEmployeesOnLeave;
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

    [HttpGet("summary")]
    [Authorize]
    public async Task<IActionResult> GetDashboardSummary()
    {
        var result = await _mediator.Send(new GetDashboardSummaryQuery());

        return Ok(result);
    }

    [HttpGet("leave-status")]
    public async Task<IActionResult> GetLeaveStatusStatistics()
    {
        var result = await _mediator.Send(new GetLeaveStatusStatisticsQuery());

        return Ok(result);
    }

    [HttpGet("monthly-trend")]
    public async Task<IActionResult> GetMonthlyLeaveTrend()
    {
        var result = await _mediator.Send(new GetMonthlyLeaveTrendQuery());

        return Ok(result);
    }


    [HttpGet("department-statistics")]
    public async Task<IActionResult> GetDepartmentStatistics()
    {
        var result = await _mediator.Send(new GetDepartmentStatisticsQuery());

        return Ok(result);
    }

    [HttpGet("leave-type-statistics")]
    public async Task<IActionResult> GetLeaveTypeStatistics()
    {
        var result = await _mediator.Send(new GetLeaveTypeStatisticsQuery());

        return Ok(result);
    }
    [HttpGet("employees-on-leave")]
    public async Task<IActionResult> GetEmployeesOnLeave()
    {
        var result = await _mediator.Send(new GetEmployeesOnLeaveQuery());

        return Ok(result);
    }
}