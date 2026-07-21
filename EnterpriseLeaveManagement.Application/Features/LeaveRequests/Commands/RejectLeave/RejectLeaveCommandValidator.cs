using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.RejectLeave;

public class RejectLeaveCommandValidator : AbstractValidator<RejectLeaveCommand>
{
    public RejectLeaveCommandValidator()
    {
        RuleFor(x => x.LeaveRequestId)
            .NotEmpty();

        RuleFor(x => x.ManagerComments)
            .NotEmpty()
            .MaximumLength(1000);
    }
}