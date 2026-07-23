using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.Dashboard.DTOs;
using EnterpriseLeaveManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetDashboardSummary;

public class GetDashboardSummaryQueryHandler: IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
{
    private readonly IApplicationDbContext _context;

    public GetDashboardSummaryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardSummaryDto> Handle(GetDashboardSummaryQuery request,CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        return new DashboardSummaryDto
        {
            TotalEmployees = await _context.Employees
                .CountAsync(x => !x.IsDeleted, cancellationToken),

            TotalDepartments = await _context.Departments
                .CountAsync(x => !x.IsDeleted, cancellationToken),

            TotalLeaveRequests = await _context.LeaveRequests
                .CountAsync(x => !x.IsDeleted, cancellationToken),

            PendingRequests = await _context.LeaveRequests
                .CountAsync(x =>
                    !x.IsDeleted &&
                    x.Status == LeaveRequestStatus.Pending,
                    cancellationToken),

            ApprovedRequests = await _context.LeaveRequests
                .CountAsync(x =>
                    !x.IsDeleted &&
                    x.Status == LeaveRequestStatus.Approved,
                    cancellationToken),

            RejectedRequests = await _context.LeaveRequests
                .CountAsync(x =>
                    !x.IsDeleted &&
                    x.Status == LeaveRequestStatus.Rejected,
                    cancellationToken),

            EmployeesOnLeaveToday = await _context.LeaveRequests
                .CountAsync(x =>
                    !x.IsDeleted &&
                    x.Status == LeaveRequestStatus.Approved &&
                    x.StartDate <= today &&
                    x.EndDate >= today,
                    cancellationToken)
        };
    }
}