using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.ApproveLeave;

public class ApproveLeaveCommandHandler : IRequestHandler<ApproveLeaveCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ApproveLeaveCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(
        ApproveLeaveCommand request,
        CancellationToken cancellationToken)
    {
        var leaveRequest = await _context.LeaveRequests
            .FirstOrDefaultAsync(
                x => x.Id == request.LeaveRequestId &&
                     !x.IsDeleted,
                cancellationToken);

        if (leaveRequest is null)
            throw new NotFoundException("Leave request not found.");

        if (leaveRequest.Status != LeaveRequestStatus.Pending)
            throw new BadRequestException("Only pending leave requests can be approved.");

        if (_currentUserService.UserId is null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        leaveRequest.Status = LeaveRequestStatus.Approved;
        leaveRequest.ApprovedBy = _currentUserService.UserId;
        leaveRequest.ApprovedOn = DateTime.UtcNow;
        leaveRequest.ManagerComments = request.ManagerComments;

        var leaveBalance = await _context.LeaveBalances
            .FirstOrDefaultAsync(
                x => x.EmployeeId == leaveRequest.EmployeeId &&
                     x.LeaveTypeId == leaveRequest.LeaveTypeId &&
                     !x.IsDeleted,
                cancellationToken);

        if (leaveBalance is null)
            throw new NotFoundException("Leave balance not found.");

        if (leaveBalance.RemainingDays < leaveRequest.NumberOfDays)
            throw new BadRequestException("Insufficient leave balance.");

        leaveBalance.UsedDays += leaveRequest.NumberOfDays;

        // Get employee to retrieve Identity UserId
        var employee = await _context.Employees
            .FirstOrDefaultAsync(
                x => x.Id == leaveRequest.EmployeeId &&
                     !x.IsDeleted,
                cancellationToken);

        if (employee is null)
            throw new NotFoundException("Employee not found.");

        _context.Notifications.Add(new Notification
        {
            UserId = employee.UserId, // Identity User Id
            Title = "Leave Request Approved",
            Message = $"Your leave request from {leaveRequest.StartDate:dd MMM yyyy} to {leaveRequest.EndDate:dd MMM yyyy} has been approved.",
            IsRead = false,
            CreatedOn = DateTime.UtcNow
        });

        await _context.SaveChangesAsync(cancellationToken);
    }
}