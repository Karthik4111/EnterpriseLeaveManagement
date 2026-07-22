using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseLeaveManagement.Domain.Common;

namespace EnterpriseLeaveManagement.Domain.Entities;

public class Employee : BaseEntity
{
    public string EmployeeCode { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }

    public DateTime DateOfJoining { get; set; }

    public string Designation { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    // Foreign Keys
    public Guid DepartmentId { get; set; }

    public Guid? ManagerId { get; set; }

    public Guid UserId { get; set; }

    // Navigation properties
    public Department Department { get; set; } = null!;

    public Employee? Manager { get; set; }

    public ICollection<Employee> Subordinates { get; set; } = new List<Employee>();

    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    public ICollection<LeaveAllocation> LeaveAllocations { get; set; } = new List<LeaveAllocation>();
}
