using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.RejectLeave;

public class RejectLeaveCommandHandler : IRequestHandler<RejectLeaveCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuditService _auditService;
    private readonly IEmailService _emailService;
    private readonly ILogger<RejectLeaveCommandHandler> _logger;

    public RejectLeaveCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IAuditService auditService,
        IEmailService emailService,
        ILogger<RejectLeaveCommandHandler> logger)
    {
        _context = context;
        _currentUserService = currentUserService;
        _auditService = auditService;
        _emailService = emailService;
        _logger = logger;
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
            UserId = employee.UserId,
            Title = "Leave Request Rejected",
            Message = $"Your leave request from {leaveRequest.StartDate:dd MMM yyyy} to {leaveRequest.EndDate:dd MMM yyyy} has been rejected.",
            IsRead = false,
            CreatedOn = DateTime.UtcNow
        });

        await _context.SaveChangesAsync(cancellationToken);

        await _auditService.LogAsync(
            action: "Leave Rejected",
            entityName: nameof(LeaveRequest),
            entityId: leaveRequest.Id,
            oldValues: "Status=Pending",
            newValues: "Status=Rejected");

        // Send email (do not fail rejection if email fails)
        try
        {
            await _emailService.SendEmailAsync(
                employee.Email,
                "Leave Request Rejected",
                $"""
                <h2>Enterprise Leave Management</h2>

                <p>Hello {employee.FirstName},</p>

                <p>Your leave request has been <b>rejected</b>.</p>

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
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to send rejection email to {Email}.",
                employee.Email);
        }
    }
}