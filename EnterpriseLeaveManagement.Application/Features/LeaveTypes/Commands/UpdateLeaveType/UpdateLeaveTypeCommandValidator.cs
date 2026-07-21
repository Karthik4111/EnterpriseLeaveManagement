using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.LeaveTypes.Commands.UpdateLeaveType;

public class UpdateLeaveTypeCommandValidator : AbstractValidator<UpdateLeaveTypeCommand>
{
    public UpdateLeaveTypeCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.DefaultDays)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.MaximumCarryForwardDays)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Name)
            .MustAsync(async (command, name, cancellationToken) =>
                !await context.LeaveTypes.AnyAsync(
                    x => x.Name == name && x.Id != command.Id,
                    cancellationToken))
            .WithMessage("Leave type name already exists.");

        RuleFor(x => x.Code)
            .MustAsync(async (command, code, cancellationToken) =>
                !await context.LeaveTypes.AnyAsync(
                    x => x.Code == code && x.Id != command.Id,
                    cancellationToken))
            .WithMessage("Leave type code already exists.");

        RuleFor(x => x)
            .Must(x => x.CarryForwardAllowed || x.MaximumCarryForwardDays == 0)
            .WithMessage("Maximum carry forward days must be 0 when carry forward is not allowed.");

        RuleFor(x => x)
            .Must(x => !x.CarryForwardAllowed || x.MaximumCarryForwardDays > 0)
            .WithMessage("Maximum carry forward days must be greater than 0 when carry forward is allowed.");
    }
}