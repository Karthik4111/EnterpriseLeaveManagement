using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;

namespace EnterpriseLeaveManagement.Application.Features.LeaveAllocations.Commands.CreateLeaveAllocation;

public class CreateLeaveAllocationCommandValidator: AbstractValidator<CreateLeaveAllocationCommand>
{
    public CreateLeaveAllocationCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty();

        RuleFor(x => x.LeaveTypeId)
            .NotEmpty();

        RuleFor(x => x.Year)
            .InclusiveBetween(2000, 2100);

        RuleFor(x => x.AllocatedDays)
            .GreaterThan(0);
    }
}