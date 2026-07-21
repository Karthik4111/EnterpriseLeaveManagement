using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommand : IRequest<CreateEmployeeResponse>
{
    public string EmployeeCode { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }

    public DateTime DateOfJoining { get; set; }

    public Guid DepartmentId { get; set; }

    public Guid? ManagerId { get; set; }

    public string Designation { get; set; } = string.Empty;
}