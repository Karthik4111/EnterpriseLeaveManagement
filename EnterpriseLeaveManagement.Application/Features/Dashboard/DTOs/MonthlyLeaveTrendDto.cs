using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLeaveManagement.Application.Features.Dashboard.DTOs;

public class MonthlyLeaveTrendDto
{
    public string Month { get; set; } = string.Empty;

    public int Count { get; set; }
}