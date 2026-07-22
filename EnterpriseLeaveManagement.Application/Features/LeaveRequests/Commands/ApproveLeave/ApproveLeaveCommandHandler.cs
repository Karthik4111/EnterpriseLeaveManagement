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
    private readonly IAuditService _auditService;
    private readonly IEmailService _emailService;

    public ApproveLeaveCommandHandler(IApplicationDbContext context,ICurrentUserService currentUserService,IAuditService auditService, IEmailService emailService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _auditService = auditService;
        _emailService = emailService;
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
            UserId = employee.UserId,
            Title = "Leave Request Approved",
            Message = $"Your leave request from {leaveRequest.StartDate:dd MMM yyyy} to {leaveRequest.EndDate:dd MMM yyyy} has been approved.",
            IsRead = false,
            CreatedOn = DateTime.UtcNow
        });

        await _context.SaveChangesAsync(cancellationToken);

        await _auditService.LogAsync(
            action: "Leave Approved",
            entityName: nameof(LeaveRequest),
            entityId: leaveRequest.Id,
            oldValues: "Status=Pending",
            newValues: "Status=Approved");

        await _emailService.SendEmailAsync(
                employee.Email,
                "Leave Request Approved",
                $"""
                <h2>Enterprise Leave Management</h2>

                <p>Hello {employee.FirstName},</p>

                <p>Your leave request has been <b>approved</b>.</p>

                <table border="1" cellpadding="8">
                    <tr><td><b>Start Date</b></td><td>{leaveRequest.StartDate:dd MMM yyyy}</td></tr>
                    <tr><td><b>End Date</b></td><td>{leaveRequest.EndDate:dd MMM yyyy}</td></tr>
                    <tr><td><b>Status</b></td><td>{leaveRequest.Status}</td></tr>
                </table>

                <p>Thank you.</p>
                """);
    }
}