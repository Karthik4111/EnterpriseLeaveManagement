using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLeaveManagement.Application.Features.Dashboard.Responses;

public class EmployeeDashboardResponse
{
    public int PendingLeaveRequests { get; set; }

    public int ApprovedLeaveRequests { get; set; }

    public int RejectedLeaveRequests { get; set; }

    public decimal RemainingLeaveBalance { get; set; }

    public int UpcomingLeaves { get; set; }
}
