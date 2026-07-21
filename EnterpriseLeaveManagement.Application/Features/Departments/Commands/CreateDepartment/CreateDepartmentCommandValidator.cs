using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace EnterpriseLeaveManagement.Application.Features.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Department name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Department code is required.")
            .MaximumLength(20);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}