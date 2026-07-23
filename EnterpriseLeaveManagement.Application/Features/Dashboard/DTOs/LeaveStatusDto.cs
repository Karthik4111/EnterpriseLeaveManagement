using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLeaveManagement.Application.Features.Dashboard.DTOs;

public class LeaveStatusDto
{
    public string Status { get; set; } = string.Empty;

    public int Count { get; set; }
}
