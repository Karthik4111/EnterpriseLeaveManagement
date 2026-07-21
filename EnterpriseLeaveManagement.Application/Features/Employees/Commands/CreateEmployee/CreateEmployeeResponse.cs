using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLeaveManagement.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeResponse
{
    public Guid EmployeeId { get; set; }

    public string Message { get; set; } = string.Empty;
}