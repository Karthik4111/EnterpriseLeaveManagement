using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLeaveManagement.Application.Features.Dashboard.DTOs;

public class DashboardSummaryDto
{
    public int TotalEmployees { get; set; }

    public int TotalDepartments { get; set; }

    public int TotalLeaveRequests { get; set; }

    public int PendingRequests { get; set; }

    public int ApprovedRequests { get; set; }

    public int RejectedRequests { get; set; }

    public int EmployeesOnLeaveToday { get; set; }
}
