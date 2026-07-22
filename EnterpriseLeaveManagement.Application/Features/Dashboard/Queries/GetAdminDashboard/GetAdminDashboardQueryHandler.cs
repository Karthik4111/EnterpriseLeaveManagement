using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.Dashboard.Responses;
using MediatR;

using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.Dashboard.Responses;
using EnterpriseLeaveManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetAdminDashboard;

public class GetAdminDashboardQueryHandler
    : IRequestHandler<GetAdminDashboardQuery, AdminDashboardResponse>
{
    private readonly IApplicationDbContext _context;

    public GetAdminDashboardQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AdminDashboardResponse> Handle(GetAdminDashboardQuery request,CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        return new AdminDashboardResponse
        {
            TotalEmployees = await _context.Employees.CountAsync(cancellationToken),

            TotalDepartments = await _context.Departments.CountAsync(cancellationToken),

            TotalLeaveTypes = await _context.LeaveTypes.CountAsync(cancellationToken),

            TotalLeaveRequests = await _context.LeaveRequests.CountAsync(cancellationToken),

            PendingLeaveRequests = await _context.LeaveRequests.CountAsync(
                x => x.Status == LeaveRequestStatus.Pending,
                cancellationToken),

            ApprovedLeaveRequests = await _context.LeaveRequests.CountAsync(
                x => x.Status == LeaveRequestStatus.Approved,
                cancellationToken),

            RejectedLeaveRequests = await _context.LeaveRequests.CountAsync(
                x => x.Status == LeaveRequestStatus.Rejected,
                cancellationToken),

            EmployeesOnLeaveToday = await _context.LeaveRequests.CountAsync(
                x => x.Status == LeaveRequestStatus.Approved &&
                     x.StartDate <= today &&
                     x.EndDate >= today,
                cancellationToken)
        };
    }
}