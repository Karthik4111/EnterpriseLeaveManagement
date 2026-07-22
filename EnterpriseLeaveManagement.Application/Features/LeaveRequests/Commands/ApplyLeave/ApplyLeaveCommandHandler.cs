using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.ApplyLeave;

public class ApplyLeaveCommandHandler : IRequestHandler<ApplyLeaveCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditService _auditService;
    private readonly IEmailService _emailService;

    public ApplyLeaveCommandHandler(IApplicationDbContext context,IAuditService auditService, IEmailService emailService)
    {
        _context = context;
        _auditService = auditService;
        _emailService = emailService;
    }

    

    public async Task<Guid> Handle(ApplyLeaveCommand request,CancellationToken cancellationToken)
    {
        // Calculate total leave days
        var numberOfDays =
            request.EndDate.DayNumber -
            request.StartDate.DayNumber + 1;

        // Validate leave balance
        var leaveBalance = await _context.LeaveBalances
            .FirstOrDefaultAsync(
                x => x.EmployeeId == request.EmployeeId &&
                     x.LeaveTypeId == request.LeaveTypeId &&
                     !x.IsDeleted,
                cancellationToken);

        if (leaveBalance is null)
            throw new Exception("Leave balance not found.");

        if (leaveBalance.RemainingDays < numberOfDays)
            throw new Exception(
                $"Insufficient leave balance. Remaining balance: {leaveBalance.RemainingDays} day(s).");

        // Get employee
        var employee = await _context.Employees
            .FirstOrDefaultAsync(
                x => x.Id == request.EmployeeId &&
                     !x.IsDeleted,
                cancellationToken);

        if (employee is null)
            throw new Exception("Employee not found.");

        // Create leave request
        var leaveRequest = new LeaveRequest
        {
            EmployeeId = request.EmployeeId,
            LeaveTypeId = request.LeaveTypeId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            NumberOfDays = numberOfDays,
            LeaveReason = request.LeaveReason,
            Status = LeaveRequestStatus.Pending
        };

        await _context.LeaveRequests.AddAsync(leaveRequest, cancellationToken);

        // Create notification using Identity User Id
        await _context.Notifications.AddAsync(new Notification
        {
            UserId = employee.UserId,
            Title = "Leave Request Submitted",
            Message = $"Your leave request from {request.StartDate:dd MMM yyyy} to {request.EndDate:dd MMM yyyy} has been submitted successfully.",
            IsRead = false,
            CreatedOn = DateTime.UtcNow
        }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        await _emailService.SendEmailAsync(
                employee.Email,
                "Leave Request Submitted",
                $"""
                <h2>Enterprise Leave Management</h2>

                <p>Hello {employee.FirstName},</p>

                <p>Your leave request has been submitted successfully.</p>

                <table border="1" cellpadding="8">
                    <tr>
                        <td><b>Start Date</b></td>
                        <td>{leaveRequest.StartDate:dd MMM yyyy}</td>
                    </tr>
                    <tr>
                        <td><b>End Date</b></td>
                        <td>{leaveRequest.EndDate:dd MMM yyyy}</td>
                    </tr>
                    <tr>
                        <td><b>Status</b></td>
                        <td>{leaveRequest.Status}</td>
                    </tr>
                </table>

                <br/>

                <p>Thank you.</p>
                """);

        await _auditService.LogAsync(
                action: "Leave Submitted",
                entityName: nameof(LeaveRequest),
                entityId: leaveRequest.Id,
                newValues: $"EmployeeId={leaveRequest.EmployeeId}, Status={leaveRequest.Status}");

        return leaveRequest.Id;
    }
}