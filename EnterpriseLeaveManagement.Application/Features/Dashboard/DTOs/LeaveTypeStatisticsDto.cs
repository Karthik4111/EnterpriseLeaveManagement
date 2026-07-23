using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLeaveManagement.Application.Features.Dashboard.DTOs;

public class LeaveTypeStatisticsDto
{
    public string LeaveType { get; set; } = string.Empty;

    public int Count { get; set; }
}