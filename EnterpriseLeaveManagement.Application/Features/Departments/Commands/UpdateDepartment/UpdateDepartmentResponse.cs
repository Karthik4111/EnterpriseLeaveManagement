using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLeaveManagement.Application.Features.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentResponse
{
    public Guid DepartmentId { get; set; }

    public string Message { get; set; } = string.Empty;
}