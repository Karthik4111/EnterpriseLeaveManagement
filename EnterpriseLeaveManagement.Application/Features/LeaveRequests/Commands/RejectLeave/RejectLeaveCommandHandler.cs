using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.RejectLeave;

public class RejectLeaveCommandHandler : IRequestHandler<RejectLeaveCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public RejectLeaveCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(
        RejectLeaveCommand request,
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
            throw new BadRequestException("Only pending leave requests can be rejected.");

        if (_currentUserService.UserId is null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        leaveRequest.Status = LeaveRequestStatus.Rejected;
        leaveRequest.ApprovedBy = _currentUserService.UserId;
        leaveRequest.ApprovedOn = DateTime.UtcNow;
        leaveRequest.ManagerComments = request.ManagerComments;

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
            Title = "Leave Request Rejected",
            Message = $"Your leave request from {leaveRequest.StartDate:dd MMM yyyy} to {leaveRequest.EndDate:dd MMM yyyy} has been rejected.",
            IsRead = false,
            CreatedOn = DateTime.UtcNow
        });

        await _context.SaveChangesAsync(cancellationToken);
    }
}