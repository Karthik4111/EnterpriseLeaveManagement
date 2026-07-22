using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLeaveManagement.Application.Features.Dashboard.Responses;

public class ManagerDashboardResponse
{
    public int TeamSize { get; set; }

    public int PendingApprovals { get; set; }

    public int ApprovedThisMonth { get; set; }

    public int RejectedThisMonth { get; set; }

    public int EmployeesOnLeaveToday { get; set; }
}