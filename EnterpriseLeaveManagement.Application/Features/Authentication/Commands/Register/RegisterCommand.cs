using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.Authentication.Commands.Register;

public class RegisterCommand : IRequest<string>
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;

    public string Role { get; set; } = "Employee";

    public Guid DepartmentId { get; set; }

    public string EmployeeCode { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }

    public DateTime DateOfJoining { get; set; }

    public string Designation { get; set; } = string.Empty;
}