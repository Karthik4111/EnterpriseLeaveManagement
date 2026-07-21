using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseLeaveManagement.Domain.Common;

namespace EnterpriseLeaveManagement.Domain.Entities;

public class LeaveBalance : BaseEntity
{
    public Guid EmployeeId { get; set; }

    public Guid LeaveTypeId { get; set; }

    public decimal TotalDays { get; set; }

    public decimal UsedDays { get; set; }

    // Navigation Properties
    public Employee Employee { get; set; } = null!;

    public LeaveType LeaveType { get; set; } = null!;

    // Computed Property
    public decimal RemainingDays => TotalDays - UsedDays;
}