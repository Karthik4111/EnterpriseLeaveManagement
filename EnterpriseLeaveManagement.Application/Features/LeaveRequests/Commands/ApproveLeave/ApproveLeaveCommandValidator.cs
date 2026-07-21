using FluentValidation;

namespace EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.ApproveLeave;

public class ApproveLeaveCommandValidator : AbstractValidator<ApproveLeaveCommand>
{
    public ApproveLeaveCommandValidator()
    {
        RuleFor(x => x.LeaveRequestId)
            .NotEmpty();

        RuleFor(x => x.ManagerComments)
            .MaximumLength(1000);
    }
}