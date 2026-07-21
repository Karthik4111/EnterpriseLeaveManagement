using EnterpriseLeaveManagement.Domain.Enums;

namespace EnterpriseLeaveManagement.Application.Features.LeaveRequests.DTOs;

public class LeaveRequestDto
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public Guid LeaveTypeId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public decimal NumberOfDays { get; set; }

    public string LeaveReason { get; set; } = string.Empty;

    public LeaveRequestStatus Status { get; set; }

    public Guid? ApprovedBy { get; set; }

    public DateTime? ApprovedOn { get; set; }

    public string? ManagerComments { get; set; }
}