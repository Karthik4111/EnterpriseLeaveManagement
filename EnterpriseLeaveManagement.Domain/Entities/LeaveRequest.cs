using EnterpriseLeaveManagement.Domain.Common;
using EnterpriseLeaveManagement.Domain.Enums;

namespace EnterpriseLeaveManagement.Domain.Entities;

public class LeaveRequest : BaseEntity
{
    public Guid EmployeeId { get; set; }

    public Guid LeaveTypeId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public decimal NumberOfDays { get; set; }

    public string LeaveReason { get; set; } = string.Empty;

    public LeaveRequestStatus Status { get; set; } = LeaveRequestStatus.Pending;

    public Guid? ApprovedBy { get; set; }

    public DateTime? ApprovedOn { get; set; }

    public string? AttachmentPath { get; set; }

    public string? ManagerComments { get; set; }

    // Navigation Properties
    public Employee Employee { get; set; } = null!;

    public LeaveType LeaveType { get; set; } = null!;
}