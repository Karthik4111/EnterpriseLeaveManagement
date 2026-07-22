using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLeaveManagement.Application.Features.Dashboard.Responses;

public class AdminDashboardResponse
{
    public int TotalEmployees { get; set; }

    public int TotalDepartments { get; set; }

    public int TotalLeaveTypes { get; set; }

    public int TotalLeaveRequests { get; set; }

    public int PendingLeaveRequests { get; set; }

    public int ApprovedLeaveRequests { get; set; }

    public int RejectedLeaveRequests { get; set; }

    public int EmployeesOnLeaveToday { get; set; }
}