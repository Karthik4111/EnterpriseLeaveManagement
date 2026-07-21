using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseLeaveManagement.Domain.Common;

namespace EnterpriseLeaveManagement.Domain.Entities;

public class LeaveType : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int DefaultDays { get; set; }

    public bool IsPaidLeave { get; set; }

    public bool CarryForwardAllowed { get; set; }

    public int MaximumCarryForwardDays { get; set; }

    public bool RequiresApproval { get; set; }

    public bool IsActive { get; set; } = true;
}