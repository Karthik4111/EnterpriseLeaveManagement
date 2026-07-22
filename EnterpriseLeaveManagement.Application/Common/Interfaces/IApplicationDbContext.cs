using EnterpriseLeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Employee> Employees { get; }

    DbSet<Department> Departments { get; }

    DbSet<LeaveType> LeaveTypes { get; }

    DbSet<LeaveBalance> LeaveBalances { get; }

    DbSet<LeaveRequest> LeaveRequests { get; }

    DbSet<LeaveAllocation> LeaveAllocations { get; }

    DbSet<Notification> Notifications { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}