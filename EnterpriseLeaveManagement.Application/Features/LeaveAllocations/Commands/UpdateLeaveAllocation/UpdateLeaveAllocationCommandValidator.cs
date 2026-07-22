using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;

namespace EnterpriseLeaveManagement.Application.Features.LeaveAllocations.Commands.UpdateLeaveAllocation;

public class UpdateLeaveAllocationCommandValidator: AbstractValidator<UpdateLeaveAllocationCommand>
{
    public UpdateLeaveAllocationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.AllocatedDays)
            .GreaterThan(0);

        RuleFor(x => x.Year)
            .InclusiveBetween(2000, 2100);
    }
}
