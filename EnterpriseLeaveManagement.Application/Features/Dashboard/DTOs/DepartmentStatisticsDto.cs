using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLeaveManagement.Application.Features.Dashboard.DTOs;

public class DepartmentStatisticsDto
{
    public string DepartmentName { get; set; } = string.Empty;

    public int EmployeeCount { get; set; }
}
