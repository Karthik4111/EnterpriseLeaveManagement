using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseLeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Employee> Employees { get; }

    DbSet<Department> Departments { get; }

    DbSet<LeaveType> LeaveTypes { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}