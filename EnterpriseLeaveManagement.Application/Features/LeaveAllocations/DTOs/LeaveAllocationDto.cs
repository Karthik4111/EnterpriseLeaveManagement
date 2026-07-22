using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLeaveManagement.Application.Features.LeaveAllocations.DTOs;

public class LeaveAllocationDto
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public string EmployeeName { get; set; } = string.Empty;

    public Guid LeaveTypeId { get; set; }

    public string LeaveTypeName { get; set; } = string.Empty;

    public int Year { get; set; }

    public int AllocatedDays { get; set; }
}