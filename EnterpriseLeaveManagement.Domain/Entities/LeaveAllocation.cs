using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnterpriseLeaveManagement.Domain.Common;

namespace EnterpriseLeaveManagement.Domain.Entities;

public class LeaveAllocation : BaseEntity
{
    public Guid EmployeeId { get; set; }

    public Guid LeaveTypeId { get; set; }

    public int Year { get; set; }

    public int AllocatedDays { get; set; }

    public Employee Employee { get; set; } = null!;

    public LeaveType LeaveType { get; set; } = null!;
}