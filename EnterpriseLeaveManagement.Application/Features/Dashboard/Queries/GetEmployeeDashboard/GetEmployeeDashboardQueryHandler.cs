using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.Dashboard.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetEmployeeDashboard;

public class GetEmployeeDashboardQueryHandler: IRequestHandler<GetEmployeeDashboardQuery, EmployeeDashboardResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetEmployeeDashboardQueryHandler(IApplicationDbContext context,ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<EmployeeDashboardResponse> Handle(GetEmployeeDashboardQuery request,CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var employee = await _context.Employees
            .FirstOrDefaultAsync(
                e => e.UserId == _currentUserService.UserId,
                cancellationToken);

        if (employee is null)
        {
            throw new Exception("Employee profile not found.");
        }

        return new EmployeeDashboardResponse
        {
            PendingLeaveRequests = await _context.LeaveRequests.CountAsync(
                x =>
                    x.EmployeeId == employee.Id &&
                    x.Status == Domain.Enums.LeaveRequestStatus.Pending,
                cancellationToken),

            ApprovedLeaveRequests = await _context.LeaveRequests.CountAsync(
                x =>
                    x.EmployeeId == employee.Id &&
                    x.Status == Domain.Enums.LeaveRequestStatus.Approved,
                cancellationToken),

            RejectedLeaveRequests = await _context.LeaveRequests.CountAsync(
                x =>
                    x.EmployeeId == employee.Id &&
                    x.Status == Domain.Enums.LeaveRequestStatus.Rejected,
                cancellationToken),

            UpcomingLeaves = await _context.LeaveRequests.CountAsync(
                x =>
                    x.EmployeeId == employee.Id &&
                    x.Status == Domain.Enums.LeaveRequestStatus.Approved &&
                    x.StartDate > today,
                cancellationToken),

            // Leave Allocation module will be implemented later
            RemainingLeaveBalance = 0
        };
    }
}
