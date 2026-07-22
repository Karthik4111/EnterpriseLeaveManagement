using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.Dashboard.Responses;
using EnterpriseLeaveManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetManagerDashboard;

public class GetManagerDashboardQueryHandler: IRequestHandler<GetManagerDashboardQuery, ManagerDashboardResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetManagerDashboardQueryHandler(IApplicationDbContext context,ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<ManagerDashboardResponse> Handle(GetManagerDashboardQuery request,CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var manager = await _context.Employees
            .FirstOrDefaultAsync(
                x => x.UserId == _currentUserService.UserId,
                cancellationToken);

        if (manager is null)
        {
            throw new Exception("Manager profile not found.");
        }

        var teamMemberIds = await _context.Employees
            .Where(x => x.ManagerId == manager.Id)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        return new ManagerDashboardResponse
        {
            TeamSize = teamMemberIds.Count,

            PendingApprovals = await _context.LeaveRequests.CountAsync(
                x => teamMemberIds.Contains(x.EmployeeId) &&
                     x.Status == LeaveRequestStatus.Pending,
                cancellationToken),

            ApprovedThisMonth = await _context.LeaveRequests.CountAsync(
                    x => teamMemberIds.Contains(x.EmployeeId) &&
                         x.Status == LeaveRequestStatus.Approved,
                    cancellationToken),

            RejectedThisMonth = await _context.LeaveRequests.CountAsync(
                    x => teamMemberIds.Contains(x.EmployeeId) &&
                         x.Status == LeaveRequestStatus.Rejected,
                    cancellationToken),

            EmployeesOnLeaveToday = await _context.LeaveRequests.CountAsync(
                x => teamMemberIds.Contains(x.EmployeeId) &&
                     x.Status == LeaveRequestStatus.Approved &&
                     x.StartDate <= today &&
                     x.EndDate >= today,
                cancellationToken)
        };
    }
}