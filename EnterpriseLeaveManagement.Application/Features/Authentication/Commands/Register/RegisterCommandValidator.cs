using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace EnterpriseLeaveManagement.Application.Features.Authentication.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.UserName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage("Passwords do not match.");

        RuleFor(x => x.Role)
            .NotEmpty();

        RuleFor(x => x.DepartmentId)
            .NotEmpty();

        RuleFor(x => x.EmployeeCode)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty();

        RuleFor(x => x.DateOfJoining)
            .NotEmpty();

        RuleFor(x => x.Designation)
            .NotEmpty()
            .MaximumLength(100);
    }
}