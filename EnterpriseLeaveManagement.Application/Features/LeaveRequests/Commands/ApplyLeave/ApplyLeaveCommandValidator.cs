using EnterpriseLeaveManagement.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.ApplyLeave;

public class ApplyLeaveCommandValidator : AbstractValidator<ApplyLeaveCommand>
{
    private readonly IApplicationDbContext _context;

    public ApplyLeaveCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .MustAsync(EmployeeExists)
            .WithMessage("Employee not found.");

        RuleFor(x => x.LeaveTypeId)
            .NotEmpty()
            .MustAsync(LeaveTypeExists)
            .WithMessage("Leave type not found.");

        RuleFor(x => x.LeaveReason)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => x.EndDate)
            .WithMessage("Start date must be before or equal to end date.");

        RuleFor(x => x)
            .MustAsync(NotHaveOverlappingLeave)
            .WithMessage("Employee already has a leave request for the selected dates.");
    }

    private async Task<bool> EmployeeExists(Guid employeeId, CancellationToken cancellationToken)
    {
        return await _context.Employees
            .AnyAsync(e => e.Id == employeeId && !e.IsDeleted, cancellationToken);
    }

    private async Task<bool> LeaveTypeExists(Guid leaveTypeId, CancellationToken cancellationToken)
    {
        return await _context.LeaveTypes
            .AnyAsync(l => l.Id == leaveTypeId && l.IsActive && !l.IsDeleted, cancellationToken);
    }

    private async Task<bool> NotHaveOverlappingLeave(ApplyLeaveCommand command,CancellationToken cancellationToken)
    {
        return !await _context.LeaveRequests.AnyAsync(l =>
            l.EmployeeId == command.EmployeeId &&
            !l.IsDeleted &&
            l.Status != Domain.Enums.LeaveRequestStatus.Rejected &&
            l.Status != Domain.Enums.LeaveRequestStatus.Cancelled &&
            command.StartDate <= l.EndDate &&
            command.EndDate >= l.StartDate,
            cancellationToken);
    }
}